
using CloudMigrationTool.Core.DataModels;
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
        /// <returns>An <see cref="IEnumerable{CloudEnumeratedItem}"/> that will enumerate every child directory found</returns>
        IAsyncEnumerable<CloudEnumeratedItem> EnumerateCloudItems();

        /// <summary>
        /// Returns an enumerable with every child directory that matches the string pattern.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{CloudEnumeratedItem}"/> that will enumerate every child directory found that matches the string pattern.</returns>
        IAsyncEnumerable<CloudEnumeratedItem> EnumerateCloudItems(string searchPattern);
        #endregion
    }
}
