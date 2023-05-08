
using System.Collections.Generic;
using System.IO;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudDirectory : ICloudItem
    {
        #region Methods
        /// <summary>
        /// Creates the directory in the cloud
        /// </summary>
        /// <returns>True if the operation was successfull, False if not</returns>
        bool Create();

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="withChildren">If true, will not fail if directory is empty.</param>
        /// <param name="permanently">If true, will delete permanently. If false, will move the directory to the trash if supported. Fail if otherwise.</param>
        /// <returns>True if the operation was successfull, False if not</returns>
        bool Delete(bool withChildren, bool permanently);

        /// <summary>
        /// Returns an enumerable with every child directory.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudDirectory}"/> that will enumerate every child directory found</returns>
        IEnumerable<ICloudDirectory> EnumerateDirectories();

        /// <summary>
        /// Returns an enumerable with every child directory that matches the string pattern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudDirectory}"/> that will enumerate every child directory found that matches the string pattern.</returns>
        IEnumerable<ICloudDirectory> EnumerateDirectories(string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable with every file within the directory.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudFile}"/> that will enumerate every child directory found</returns>
        IEnumerable<ICloudFile> EnumerateFiles();

        /// <summary>
        /// Returns an enumerable with every file within the directory that matches the string pattern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudFile}"/> that will enumerate every child directory found that matches the string pattern.</returns>
        IEnumerable<ICloudFile> EnumerateFiles(string searchPattern, SearchOption searchOption);
        #endregion
    }
}
