using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.Interfaces
{
    interface ICloudSource
    {
        /// <summary>
        /// Contains the currently logged in user info to display in the app
        /// </summary>
        UserInfo LoggedInUserInfo { get; }

        /// <summary>
        /// Contains the cloud info to display in the app
        /// </summary>
        CloudInfo CloudInfo { get; }

        /// <summary>
        /// True if user has been already authenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Initiates the process to authenticate the user
        /// </summary>
        /// <param name="afterAuthentication">Action to execute after authentication is done. True if the authentication is complete. False if there was a failure.</param>
        void Authenticate(Action<bool> afterAuthentication);

        /// <summary>
        /// Logs out of the cloud provider
        /// </summary>
        /// <returns></returns>
        bool Logout();

        /// <summary>
        /// Gets a cloud directory located on the specified path
        /// </summary>
        /// <param name="fileParameters">Parameters needed to get the directory</param>
        /// <returns>A ICloudDirectory instance containing the information and methods to access the directory</returns>
        ICloudDirectory GetCloudDirectory(CloudItemParameters fileParameters);

        /// <summary>
        /// Searches for cloud directories or files based on the specified search options
        /// </summary>
        /// <param name="searchParams">Contains the search parameters for this operation</param>
        /// <returns>The search results for the specified parameters</returns>
        SearchResults SearchCloudDirectory(SearchParameters searchParameters);

        /// <summary>
        /// Gets a cloud file located on the specified path
        /// </summary>
        /// <param name="fileParameters">Parameters needed to get the file</param>
        /// <returns>A ICloudFile instance containing the information and methods to access the file</returns>
        ICloudFile GetCloudFile(CloudItemParameters fileParameters);

        /// <summary>
        /// Gets the root cloud directory
        /// </summary>
        /// <returns>A ICloudDirectory instance containing the information and methods to access the root directory</returns>
        ICloudDirectory GetRootDirectory();
    }
}
