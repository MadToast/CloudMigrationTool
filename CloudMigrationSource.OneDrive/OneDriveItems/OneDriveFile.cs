using CloudMigrationTool.Core.Interfaces;
using CloudMigrationTool.Core.SearchObjects;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CoreConstants = CloudMigrationTool.Core.Constants;

namespace CloudMigrationSource.OneDrive.OneDriveItems
{
    internal class OneDriveFile : ICloudFile
    {
        #region Public Properties
        public string Extension => Path.GetExtension(_fileItem.Name);

        public string MimeType => _fileItem.FileObject.MimeType;

        public IReadOnlyDictionary<string, object> Attributes => throw new NotImplementedException();

        public DateTimeOffset? CreationDateTime => _fileItem.CreatedDateTime.Value;

        public DateTimeOffset? LastModifiedDateTime => _fileItem.LastModifiedDateTime.HasValue ?
            _fileItem.LastModifiedDateTime.Value :
            _fileItem.CreatedDateTime.Value;

        public string FullName => $"{_fileItem.ParentReference.Path?.Substring(Constants.PathPrefix.Length) ?? "/"}{_fileItem.Name}";

        public string Name => _fileItem.Name;

        public long Length => _fileItem.Size ?? CoreConstants.UndeterminedLength;
        #endregion

        #region private variables
        private DriveItem _fileItem;
        private OneDriveCloudSource _cloudSource;
        private OneDriveDirectory _parent;
        #endregion

        internal OneDriveFile(DriveItem fileItem, OneDriveCloudSource cloudSource)
        {
            _fileItem = fileItem;
            _cloudSource = cloudSource;
        }

        public async Task<ICloudDirectory> GetParent()
        {
            if (_parent == null && !string.IsNullOrWhiteSpace(_fileItem.ParentReference.Id))
            {
                var parentItem = await _cloudSource.GetDriveItemAsync(new CloudItemParameters()
                {
                    ID = _fileItem.ParentReference.Id,
                });

                _parent = new OneDriveDirectory(parentItem, _cloudSource);
            }

            return _parent;
        }

        public async Task<Stream> Open()
        {
            return await _cloudSource.OpenFileStream(_fileItem);
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
