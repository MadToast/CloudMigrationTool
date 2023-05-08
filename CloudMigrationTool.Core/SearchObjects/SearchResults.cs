using CloudMigrationTool.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.SearchObjects
{
    public class SearchResults
    {
        public ICloudFile[] CloudFiles { get; set; }
        public ICloudDirectory[] CloudDirectories { get; set; }
        public string NextLink { get; set; }
    }
}
