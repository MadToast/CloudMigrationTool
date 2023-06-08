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
        private string Id;
        private DriveItem directoryItem;
        private GraphServiceClient graphServiceClient;
        private OneDriveCloudSource oneDriveCloudSource;
        #endregion

        internal OneDriveDirectory(OneDriveCloudSource _oneDriveCloudSource, GraphServiceClient _graphServiceClient)
        {
            this.oneDriveCloudSource = _oneDriveCloudSource;
            this.graphServiceClient = _graphServiceClient;
        }

        public Task<bool> Delete(bool withChildren, bool permanently)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICloudDirectory>> EnumerateDirectories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICloudDirectory>> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICloudFile>> EnumerateFiles()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICloudFile>> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAttributes(IDictionary<string, object> attributes)
        {
            throw new NotImplementedException();
        }
    }
}
