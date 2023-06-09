using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.SearchObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationTool.Core.Interfaces
{
    public abstract class CloudSource
    {
        /// <summary>
        /// Contains the currently logged in user info to display in the app
        /// </summary>
        public abstract UserInfo LoggedInUserInfo { get; }

        /// <summary>
        /// Contains the cloud info to display in the app
        /// </summary>
        public abstract CloudInfo CloudInfo { get; }

        /// <summary>
        /// True if user has been already authenticated
        /// </summary>
        public abstract bool IsAuthenticated { get; }

        protected bool _isConsoleMode;

        public CloudSource(bool isConsoleMode)
        {
            _isConsoleMode = isConsoleMode;
        }

        /// <summary>
        /// Initiates the process to authenticate the user
        /// </summary>
        /// <param name="afterAuthentication">Action to execute after authentication is done. True if the authentication is complete. False if there was a failure.</param>
        public abstract Task<bool> Authenticate();

        /// <summary>
        /// Logs out of the cloud provider
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> Logout();

        /// <summary>
        /// Gets a cloud directory located on the specified path
        /// </summary>
        /// <param name="fileParameters">Parameters needed to get the directory</param>
        /// <returns>A ICloudDirectory instance containing the information and methods to access the directory</returns>
        public abstract Task<ICloudDirectory> GetCloudDirectory(CloudItemParameters fileParameters);

        /// <summary>
        /// Gets a cloud file located on the specified path
        /// </summary>
        /// <param name="fileParameters">Parameters needed to get the file</param>
        /// <returns>A ICloudFile instance containing the information and methods to access the file</returns>
        public abstract Task<ICloudFile> GetCloudFile(CloudItemParameters fileParameters);

        /// <summary>
        /// Gets the root cloud directory
        /// </summary>
        /// <returns>A ICloudDirectory instance containing the information and methods to access the root directory</returns>
        public abstract Task<ICloudDirectory> GetRootDirectory();

        /// <summary>
        /// Creates a directory at the specified path
        /// </summary>
        /// <returns>A ICloudDirectory instance containing the information and methods to access the root directory</returns>
        public abstract Task<ICloudDirectory> CreateDirectory(string path);

        /// <summary>
        /// Creates the file in the cloud and returns a stream
        /// </summary>
        /// <param name="inputStream">Input stream with the file content</param>
        /// <param name="overwriteIfExists">Overwrites the file with an empty one if it exists</param>
        /// <param name="progressCallback">Callback for any progress update</param>
        public abstract Task<ICloudFile> UploadFile(ICloudDirectory destination, ICloudFile file, bool overwriteIfExists = false, IProgress<long> progressCallback = null);
    }
}
