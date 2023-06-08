
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudDirectory : ICloudItem
    {
        #region Methods
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="withChildren">If true, will not fail if directory is empty.</param>
        /// <param name="permanently">If true, will delete permanently. If false, will move the directory to the trash if supported. Fail if otherwise.</param>
        /// <returns>True if the operation was successfull, False if not</returns>
        Task<bool> Delete(bool withChildren, bool permanently);

        /// <summary>
        /// Returns an enumerable with every child directory.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudDirectory}"/> that will enumerate every child directory found</returns>
        Task<IEnumerable<ICloudDirectory>> EnumerateDirectories();

        /// <summary>
        /// Returns an enumerable with every child directory that matches the string pattern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudDirectory}"/> that will enumerate every child directory found that matches the string pattern.</returns>
        Task<IEnumerable<ICloudDirectory>> EnumerateDirectories(string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable with every file within the directory.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudFile}"/> that will enumerate every child directory found</returns>
        Task<IEnumerable<ICloudFile>> EnumerateFiles();

        /// <summary>
        /// Returns an enumerable with every file within the directory that matches the string pattern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{ICloudFile}"/> that will enumerate every child directory found that matches the string pattern.</returns>
        Task<IEnumerable<ICloudFile>> EnumerateFiles(string searchPattern, SearchOption searchOption);
        #endregion
    }
}
