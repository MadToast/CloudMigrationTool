
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudDirectory : ICloudItem
    {
        #region Methods
        /// <summary>
        /// Creates a child directory
        /// </summary>
        /// <returns>The reference to the new directory</returns>
        Task<ICloudDirectory> CreateChildDirectory(string directoryName);

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
