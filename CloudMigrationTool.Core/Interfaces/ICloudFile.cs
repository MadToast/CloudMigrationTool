using System;
using System.Collections.Generic;
using System.IO;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudFile : ICloudItem
    {
        #region Properties
        /// <summary>
        /// Returns the extension of the file
        /// </summary>
        string Extension { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the file in the cloud and returns a stream
        /// </summary>
        /// <param name="overwriteIfExists">Overwrites the file with an empty one if it exists</param>
        Stream Upload(bool overwriteIfExists);

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <param name="permanent">If true, will delete permanently. If false, will move the directory to the trash if supported. Fail if otherwise.</param>
        /// <returns>True if the operation was successfull, False if not</returns>
        bool Delete(bool permanent);

        /// <summary>
        /// Opens a FileStream with the specified mode
        /// </summary>
        /// <param name="mode">A <see cref="FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
        Stream Open(FileMode mode);
        #endregion
    }
}
