using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.Interfaces;
using CloudMigrationTool.Core.SearchObjects;
using System;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Graph.Models;
using System.IO;
using CloudMigrationTool.Core.Extensions;
using CloudMigrationSource.OneDrive.OneDriveItems;
using Microsoft.Graph.Core.Requests;
using CloudMigrationTool.Core.Exceptions;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Drives.Item.Items.Item.SearchWithQ;
using CloudMigrationSource.OneDrive.DataModels;
using Microsoft.Kiota.Abstractions.Serialization;

namespace CloudMigrationSource.OneDrive
{
    public class OneDriveCloudSource : CloudSource
    {
        #region Properties
        public override UserInfo LoggedInUserInfo => _authenticationToken?.UserInfo;

        public override CloudInfo CloudInfo => _oneDriveCloudInfo;

        public override bool IsAuthenticated => _authenticationToken != null;
        #endregion

        #region Private Variables
        private AuthenticationToken _authenticationToken;

        private IPublicClientApplication _identityClient;

        private GraphServiceClient _graphServiceClient;

        private CloudInfo _oneDriveCloudInfo;
        private DriveItem _rootDirectory;
        #endregion

        #region Constructors
        public OneDriveCloudSource(bool isConsoleMode) : base(isConsoleMode)
        {
            _identityClient = PublicClientApplicationBuilder.Create(Constants.ApplicationId)
                .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                .WithRedirectUri("http://localhost")
                .Build();
        }
        #endregion

        #region Public Methods
        public override async Task<bool> Authenticate()
        {
            var accounts = await _identityClient.GetAccountsAsync();
            AuthenticationResult result = null;
            try
            {
                result = await _identityClient
                    .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                if (_isConsoleMode)
                {
                    result = await _identityClient
                        .AcquireTokenWithDeviceCode(Constants.Scopes, (deviceCode) =>
                        {
                            Console.WriteLine(deviceCode.Message);
                            return Task.FromResult(0);
                        })
                        .ExecuteAsync();
                }
                else
                {
                    result = await _identityClient
                        .AcquireTokenInteractive(Constants.Scopes)
                        .ExecuteAsync();
                }
            }
            catch (Exception ex)
            {
                // Display the error text - probably as a pop-up
                Debug.WriteLine($"Error: Authentication failed: {ex.Message}");
                return false;
            }

            var graphHttpClient = new HttpClient();
            graphHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result?.AccessToken);
            _graphServiceClient = new GraphServiceClient(graphHttpClient);

            string photoFile = null;
            try
            {
                var photoDetails = await _graphServiceClient.Me.Photos["48x48"].GetAsync();
                var contentType = photoDetails?.BackingStore?.Get<string>("@odata.mediaContentType");

                if (!string.IsNullOrEmpty(photoDetails?.Id) &&
                    Constants.AllowedImageTypes.Any((x) => x.Equals(contentType, StringComparison.OrdinalIgnoreCase)))
                {
                    using (var photoStream = await _graphServiceClient.Me.Photos["48x48"].Content.GetAsync())
                    {
                        photoFile = Path.GetTempFileName() + Constants.ImageTypeExtensionMappping[contentType];
                        using (var photoFileStream = File.OpenWrite(photoFile))
                        {
                            photoStream.CopyTo(photoFileStream);
                        }
                    }
                }

                _oneDriveCloudInfo = await SetupOneDriveCloudInfoAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: Graph call failed: {ex.Message}");
                return false;
            }

            _authenticationToken = new AuthenticationToken
            {
                ExpiresOn = result?.ExpiresOn ?? DateTimeOffset.MinValue,
                Token = result?.AccessToken ?? "",
                UserInfo = new UserInfo(
                    id: result?.Account?.HomeAccountId?.ObjectId,
                    name: result?.Account?.Username,
                    displayName: result?.ClaimsPrincipal?
                    .FindAll((claim) => claim.Type.Equals("name", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.Value ?? result?.Account?.Username,
                    email: result?.Account?.Username,
                    photoFile: photoFile
                )
            };

            return true;
        }

        public override async Task<bool> Logout()
        {
            try
            {
                var accounts = await _identityClient.GetAccountsAsync();

                foreach (var account in accounts)
                {
                    await _identityClient.RemoveAsync(account);
                }

                _authenticationToken = null;
                _graphServiceClient = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: Logout call failed: {ex.Message}");
                return false;
            }

            return true;
        }

        public override async Task<ICloudDirectory> GetCloudDirectory(CloudItemParameters directoryParameters)
        {
            var driveItem = await GetDriveItemAsync(directoryParameters);

            if (driveItem.FileObject != null)
                throw new DirectoryWasFileException();

            return new OneDriveDirectory(driveItem, this);
        }

        public override async Task<ICloudFile> GetCloudFile(CloudItemParameters fileParameters)
        {
            var driveItem = await GetDriveItemAsync(fileParameters);

            if (driveItem.Folder != null)
                throw new FileWasDirectoryException();

            return new OneDriveFile(driveItem, this);
        }

        public override Task<ICloudDirectory> GetRootDirectory()
        {
            CheckAuthenticationStatus();

            return Task.FromResult((ICloudDirectory)new OneDriveDirectory(_rootDirectory, this));
        }

        public override async Task<ICloudDirectory> CreateDirectory(string path)
        {
            CheckAuthenticationStatus();

            //Initialize a sanitized path
            var sanitizedPath = path;

            // Remove initial slash
            if (path.StartsWith("/"))
                sanitizedPath = path.Substring(1);

            if (sanitizedPath.EndsWith("/"))
                sanitizedPath = sanitizedPath.Substring(0, sanitizedPath.Length - 1);

            // Sanity check
            if (string.IsNullOrWhiteSpace(sanitizedPath))
                throw new InvalidOperationException("Path cannot be empty!");

            var slashLastIndex = sanitizedPath.LastIndexOf("/");

            DriveItem directoryItem;
            if (slashLastIndex == -1)
            {
                // Create at root
                directoryItem = await _graphServiceClient
                    .Drives[_oneDriveCloudInfo.Id]
                    .Items[_rootDirectory.Id]
                    .Children
                    .PostAsync(new DriveItem()
                    {
                        Name = sanitizedPath
                    });
            }
            else
            {
                var parent = sanitizedPath.Substring(0, slashLastIndex);
                var folderName = sanitizedPath.Substring(slashLastIndex + 1);
                directoryItem = await _graphServiceClient
                    .Drives[_oneDriveCloudInfo.Id]
                    .Root
                    .ItemWithPath($"/{parent}")
                    .Children
                    .PostAsync(new DriveItem()
                    {
                        Name = folderName
                    });
            }

            return new OneDriveDirectory(directoryItem, this);
        }

        public override async Task<ICloudFile> UploadFile(ICloudDirectory destination, ICloudFile file, bool overwriteIfExists = false, IProgress<long> progressCallback = null)
        {
            CheckAuthenticationStatus();

            // Use properties to specify the conflict behavior
            // in this case, replace
            var uploadSessionRequestBody = new CreateUploadSessionPostRequestBody()
            {
                Item = new DriveItemUploadableProperties()
                {
                    FileSize = file.Length,
                    Name = file.Name
                }
            };

            if (overwriteIfExists)
            {
                uploadSessionRequestBody.Item.AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "replace" }
                };
            };

            var uploadSession = await _graphServiceClient
                .Drives[_oneDriveCloudInfo.Id]
                .Items[_rootDirectory.Id]
                .CreateUploadSession
                .PostAsync(uploadSessionRequestBody);

            using (var inputStream = await file.Open())
            {
                var fileUploadTask = new LargeFileUploadTask<DriveItem>(
                    uploadSession: uploadSession,
                    uploadStream: inputStream,
                    requestAdapter: _graphServiceClient.RequestAdapter);

                var totalLength = inputStream.Length;

                // Create a callback that is invoked after each slice is uploaded
                IProgress<long> progress = new Progress<long>(prog =>
                {
                    Console.WriteLine($"Uploaded {prog} bytes of {totalLength} bytes");

                    if (progressCallback != null)
                    {
                        progressCallback?.Report(prog);
                    }
                });

                try
                {
                    // Upload the file
                    var uploadResult = await fileUploadTask.UploadAsync(progress);

                    Console.WriteLine(uploadResult.UploadSucceeded ?
                        $"Upload complete, item ID: {uploadResult.ItemResponse.Id}" :
                        "Upload failed");

                    return new OneDriveFile(uploadResult.ItemResponse, this);
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Error uploading: {ex.ToString()}");
                    throw;
                }
            }
        }
        #endregion

        #region Private Methods
        private async IAsyncEnumerable<TReturn> IterateThroughResponse<TReturn, TCollectionPage>(TCollectionPage response)
            where TCollectionPage : IParsable, IAdditionalDataHolder, new()
        {
            var itemsIterated = new List<TReturn>();

            var itemsIterator = PageIterator<TReturn, TCollectionPage>
                .CreatePageIterator(
                    _graphServiceClient,
                    response,
                    (iteratedItem) =>
                    {
                        itemsIterated.Add(iteratedItem);
                        return true;
                    });

            await itemsIterator.IterateAsync();

            while (itemsIterator.State != PagingState.Complete)
            {
                foreach (var iteratedItem in itemsIterated)
                {
                    yield return iteratedItem;
                }

                itemsIterated.Clear();
                await itemsIterator.ResumeAsync();
            }
        }
        private async Task<CloudInfo> SetupOneDriveCloudInfoAsync()
        {
            ExceptionHelpers.ThrowIfNull(_authenticationToken, nameof(_authenticationToken));
            ExceptionHelpers.ThrowIfNull(_graphServiceClient, nameof(_graphServiceClient));

            var _userDrive = await _graphServiceClient.Me.Drive.GetAsync();
            _rootDirectory = _userDrive.Root;

            return new CloudInfo(
                    id: _userDrive.Id,
                    name: _userDrive.DriveType,
                    totalSize: _userDrive.Quota.Total ?? -1, // -1 means Not provided
                    usedSize: _userDrive.Quota.Used ?? -1,
                    remainingSize: _userDrive.Quota.Remaining ?? -1
                );
        }

        private void CheckAuthenticationStatus()
        {
            if (!IsAuthenticated)
            {
                throw new CloudSourceNotAuthenticatedException("User has not authenticated yet!");
            }
        }

        #endregion

        #region Internal Methods
        internal async Task<DriveItem> GetDriveItemAsync(CloudItemParameters parameters)
        {
            if (parameters == null ||
                (string.IsNullOrWhiteSpace(parameters.ID) &&
                 string.IsNullOrWhiteSpace(parameters.Path)))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            CheckAuthenticationStatus();

            if (parameters.Path?.Trim().Equals("/") == true ||
                parameters.ID?.Equals(_rootDirectory.Id) == true)
            {
                // It's the root.
                return _rootDirectory;
            }

            if (!string.IsNullOrWhiteSpace(parameters.ID))
            {
                return await _graphServiceClient
                                .Drives[_oneDriveCloudInfo.Id]
                                .Items[parameters.ID]
                                .GetAsync();
            }

            return await _graphServiceClient
                .Drives[_oneDriveCloudInfo.Id]
                .Root
                .ItemWithPath(parameters.Path)
                .GetAsync();
        }
        internal async Task<Stream> OpenFileStream(DriveItem fileItem)
        {
            CheckAuthenticationStatus();

            if (fileItem.FileObject == null)
                throw new FileWasDirectoryException();

            return await _graphServiceClient
                .Drives[_oneDriveCloudInfo.Id]
                .Items[fileItem.Id]
                .Content.GetAsync();
        }
        internal async IAsyncEnumerable<DriveItem> EnumerateDriveItems(DriveItem directoryItem, string searchPattern)
        {
            var directoryChildren = await _graphServiceClient
                    .Drives[_oneDriveCloudInfo.Id]
                    .Items[directoryItem.Id]
                    .SearchWithQ(searchPattern)
                    .GetAsync();

            await foreach (var item in IterateThroughResponse<DriveItem, SearchWithQResponse>(directoryChildren))
            {
                yield return item;
            }
        }
        internal async IAsyncEnumerable<DriveItem> EnumerateDriveItems(DriveItem directoryItem)
        {
            var directoryChildren = await _graphServiceClient
                    .Drives[_oneDriveCloudInfo.Id]
                    .Items[directoryItem.Id]
                    .Children
                    .GetAsync();

            await foreach (var item in IterateThroughResponse<DriveItem, DriveItemCollectionResponse>(directoryChildren))
            {
                yield return item;
            }
        }
        #endregion
    }
}
