using CloudMigrationTool.Core.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationSource.OneDrive.OneDriveItems
{
    internal class OneDriveFile : ICloudFile
    {
        public string Extension => throw new NotImplementedException();

        public string MimeType => throw new NotImplementedException();

        public IDictionary<string, object> Attributes => throw new NotImplementedException();

        public DateTime CreationTime => throw new NotImplementedException();

        public DateTime CreationTimeUtc => throw new NotImplementedException();

        public ICloudDirectory Parent => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        #region private variables
        private DriveItem _fileItem;
        private OneDriveCloudSource _cloudSource;
        #endregion

        internal OneDriveFile(DriveItem fileItem, OneDriveCloudSource cloudSource)
        {
            _fileItem = fileItem;
            _cloudSource = cloudSource;
        }

        public Stream Open(FileMode mode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
