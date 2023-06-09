using CloudMigrationTool.Core.Enums;
using CloudMigrationTool.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public class CloudEnumeratedItem
    {
        public CloudItemType CloudItemType => CloudFile != null ? CloudItemType.File : CloudItemType.Directory;
        public ICloudFile CloudFile { get; set; }
        public ICloudDirectory CloudDirectory { get; set; }
    }
}
