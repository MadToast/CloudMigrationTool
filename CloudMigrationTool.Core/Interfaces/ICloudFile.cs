using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudFile : ICloudItem
    {
        #region Properties
        /// <summary>
        /// Returns the extension of the file
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Returns the MimeType of the file
        /// </summary>
        string MimeType { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Opens a FileStream with the specified mode
        /// </summary>
        Task<Stream> Open();
        #endregion
    }
}
