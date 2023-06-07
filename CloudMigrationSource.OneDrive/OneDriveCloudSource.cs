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

namespace CloudMigrationSource.OneDrive
{
    public class OneDriveCloudSource : ICloudSource
    {
        #region Properties
        public UserInfo LoggedInUserInfo => _authenticationToken?.UserInfo;

        public CloudInfo CloudInfo => _userCloudInfo;

        public bool IsAuthenticated => _authenticationToken != null;
        #endregion

        #region Private Variables
        private AuthenticationToken _authenticationToken;

        private IPublicClientApplication _identityClient;

        private GraphServiceClient _graphServiceClient;

        private OneDriveCloudInfo _userCloudInfo;
        #endregion

        #region Constructors
        public OneDriveCloudSource()
        {
            _identityClient = PublicClientApplicationBuilder.Create(Constants.ApplicationId)
                .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                .WithRedirectUri("http://localhost")
                .Build();
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
            } catch (Exception ex)
            {
                Debug.WriteLine($"Error: Graph call failed: {ex.Message}");
                return false;
            }

            _authenticationToken =  new AuthenticationToken
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

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<ICloudDirectory> GetCloudDirectory(CloudItemParameters fileParameters)
        {
            throw new NotImplementedException();
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
        #endregion

        #region Private Methods
        #endregion
    }
}
