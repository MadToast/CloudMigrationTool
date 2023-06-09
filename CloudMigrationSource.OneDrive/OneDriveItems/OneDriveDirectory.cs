using Azure.Core;
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
        public IDictionary<string, object> Attributes => throw new NotImplementedException();

        public DateTime CreationTime => throw new NotImplementedException();

        public DateTime CreationTimeUtc => throw new NotImplementedException();

        public ICloudDirectory Parent => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        #region private variables
        private DriveItem _directoryItem;
        private OneDriveCloudSource _cloudSource;
        private GraphServiceClient _graphClient;
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

        public Task<IEnumerable<ICloudDirectory>> EnumerateDirectories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICloudFile>> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
