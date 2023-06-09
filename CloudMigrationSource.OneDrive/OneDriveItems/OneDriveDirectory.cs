using Azure.Core;
using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationSource.OneDrive.OneDriveItems
{
    internal class OneDriveDirectory : ICloudDirectory
    {
        public IDictionary<string, object> Attributes => throw new NotSupportedException();

        public DateTimeOffset CreationDateTime => _directoryItem.CreatedDateTime.Value;

        public DateTimeOffset LastModifiedDateTime => _directoryItem.LastModifiedDateTime.HasValue ? 
            _directoryItem.LastModifiedDateTime.Value :
            _directoryItem.CreatedDateTime.Value;

        public ICloudDirectory Parent => _parent;

        public string FullName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        #region private variables
        private DriveItem _directoryItem;
        private OneDriveDirectory _parent;
        private OneDriveCloudSource _cloudSource;
        private GraphServiceClient _graphClient;
        #endregion

        internal OneDriveDirectory(DriveItem directoryItem, DriveItem parentItem, OneDriveCloudSource cloudSource)
        {
            _directoryItem = directoryItem;
            _cloudSource = cloudSource;
            _parentItem = parentItem;
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
    }
}
