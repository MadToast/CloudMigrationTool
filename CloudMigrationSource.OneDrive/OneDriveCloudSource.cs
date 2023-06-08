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

namespace CloudMigrationSource.OneDrive
{
    public class OneDriveCloudSource : ICloudSource
    {
        #region Properties
        public UserInfo LoggedInUserInfo => _authenticationToken?.UserInfo;

        public CloudInfo CloudInfo => _oneDriveCloudInfo;

        public bool IsAuthenticated => _authenticationToken != null;
        #endregion

        #region Internal Only Properties
        internal Drive UserDrive => _userDrive;
        #endregion

        #region Private Variables
        private AuthenticationToken _authenticationToken;

        private IPublicClientApplication _identityClient;

        private GraphServiceClient _graphServiceClient;

        private OneDriveCloudInfo _oneDriveCloudInfo;
        private Drive _userDrive;
        #endregion

        #region Constructors
        public OneDriveCloudSource()
        {
            _identityClient = PublicClientApplicationBuilder.Create(Constants.ApplicationId)
                .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                .WithRedirectUri("http://localhost")
                .Build();

            _oneDriveCloudInfo = new OneDriveCloudInfo(-1, -1, -1);
        }
        #endregion

        #region Public Methods
        public async Task<bool> Authenticate()
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
                result = await _identityClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .ExecuteAsync();
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

        public async Task<bool> Logout()
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

        public Task<ICloudDirectory> GetCloudDirectory(CloudItemParameters directoryParameters)
        {
            var oneDrivePath = $"root:{directoryParameters.Path}"
        }

        public Task<SearchResults> SearchCloudDirectory(SearchParameters searchParameters)
        {
            throw new NotImplementedException();
        }

        public Task<ICloudFile> GetCloudFile(CloudItemParameters fileParameters)
        {
            throw new NotImplementedException();
        }

        public Task<ICloudDirectory> GetRootDirectory()
        {
            throw new NotImplementedException();
        }
        public Task<ICloudDirectory> CreateDirectory()
        {
            if (IsAuthenticated && _graphServiceClient != null)
            {
                if (Exists)
                {
                    return true;
                }

                var driveItem = new DriveItem()
                {
                    Name = Name,
                    Folder = new Folder
                    {
                    }
                };

                string parentId;

                // Check if this is a folder at root
                if (FullName.Count((x) => x.Equals("/")) == 1)
                {
                    var root = await graphServiceClient
                        .Drives[oneDriveCloudSource.UserDrive.Id]
                        .Root
                        .GetAsync();

                    parentId = root.Id;
                }
                else if (Parent == null)
                {
                    Parent = new OneDriveDirectory(oneDriveCloudSource, graphServiceClient);
                    Parent.
                }



                if (Parent.Exists || !Parent.Exists && (await Parent.Create()))
                {
                    var oneDriveParent = (OneDriveDirectory)Parent;

                    var result = graphServiceClient
                        .Drives[oneDriveCloudSource.UserDrive.Id]
                        .Items[oneDriveParent.Id]
                        .Children
                        .PostAsync(driveItem);
                }
            }
        }
        #endregion

        #region Private Methods
        private async Task<OneDriveCloudInfo> SetupOneDriveCloudInfoAsync()
        {
            ExceptionHelpers.ThrowIfNull(_authenticationToken, nameof(_authenticationToken));
            ExceptionHelpers.ThrowIfNull(_graphServiceClient, nameof(_graphServiceClient));

            _userDrive = await _graphServiceClient.Me.Drive.GetAsync();

            return new OneDriveCloudInfo(
                    totalSize: _userDrive.Quota.Total ?? -1, // -1 means Not provided
                    usedSize: _userDrive.Quota.Used ?? -1,
                    remainingSize: _userDrive.Quota.Remaining ?? -1
                );
        }

        private async Task<DriveItem> GetDriveItemIfExists(string pathToItem)
        {
            if (IsAuthenticated && _graphServiceClient != null)
            {
                try
                {
                    var driveItem = await _graphServiceClient
                    .Drives[UserDrive.Id]
                    .Root
                    .ItemWithPath(pathToItem)
                    .GetAsync();

                    return driveItem;
                } catch (ServiceException ex)
                {
                    if (ex.ResponseStatusCode == 404)
                    {
                        return null; // No file exists
                    }

                    //Throw the exception for anything else
                    throw;
                }
            }

            throw new InvalidOperationException("Attempted to get a Drive item without authentication.");
        }

        #endregion
    }
}
