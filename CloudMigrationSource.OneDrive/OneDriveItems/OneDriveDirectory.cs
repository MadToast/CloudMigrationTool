using Azure.Core;
using CloudMigrationTool.Core;
using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.Interfaces;
using CloudMigrationTool.Core.SearchObjects;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreConstants = CloudMigrationTool.Core.Constants;

namespace CloudMigrationSource.OneDrive.OneDriveItems
{
    internal class OneDriveDirectory : ICloudDirectory
    {
        public IReadOnlyDictionary<string, object> Attributes => new Dictionary<string, object>();

        public DateTimeOffset? CreationDateTime => _directoryItem.CreatedDateTime.Value;

        public DateTimeOffset? LastModifiedDateTime => _directoryItem.LastModifiedDateTime.HasValue ? 
            _directoryItem.LastModifiedDateTime.Value :
            _directoryItem.CreatedDateTime.Value;

        public string FullName => $"{_directoryItem.ParentReference.Path?.Substring(Constants.PathPrefix.Length) ?? "/"}{_directoryItem.Name}";

        public string Name => _directoryItem.Name;

        public long Length => _directoryItem.Size ?? CoreConstants.UndeterminedLength;

        #region private variables
        private OneDriveCloudSource _cloudSource;
        private DriveItem _directoryItem;
        private OneDriveDirectory _parent;
        #endregion

        internal OneDriveDirectory(DriveItem directoryItem, OneDriveCloudSource cloudSource)
        {
            _directoryItem = directoryItem;
            _cloudSource = cloudSource;
        }

        public Task<ICloudDirectory> CreateChildDirectory(string directoryName)
        {
            return _cloudSource.CreateDirectory($"{FullName}/{directoryName}");
        }

        public async IAsyncEnumerable<CloudEnumeratedItem> EnumerateCloudItems()
        {
            await foreach (var driveItem in _cloudSource.EnumerateDriveItems(_directoryItem))
            {
                yield return new CloudEnumeratedItem()
                {
                    CloudDirectory = driveItem.Folder == null ? null :
                        new OneDriveDirectory(driveItem, _cloudSource),
                    CloudFile = driveItem.FileObject == null ? null :
                        new OneDriveFile(driveItem, _cloudSource)
                };
            }
        }

        public async IAsyncEnumerable<CloudEnumeratedItem> EnumerateCloudItems(string searchPattern)
        {
            await foreach (var driveItem in _cloudSource.EnumerateDriveItems(_directoryItem, searchPattern))
            {
                yield return new CloudEnumeratedItem()
                {
                    CloudDirectory = driveItem.Folder == null ? null :
                        new OneDriveDirectory(driveItem, _cloudSource),
                    CloudFile = driveItem.FileObject == null ? null :
                        new OneDriveFile(driveItem, _cloudSource)
                };
            }
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotSupportedException();
        }

        public async Task<ICloudDirectory> GetParent()
        {
            if (_parent == null && !string.IsNullOrWhiteSpace(_directoryItem.ParentReference.Id))
            {
                var parentItem = await _cloudSource.GetDriveItemAsync(new CloudItemParameters()
                {
                    ID = _directoryItem.ParentReference.Id,
                });

                _parent = new OneDriveDirectory(parentItem, _cloudSource);
            }

            return _parent;
        }
    }
}
