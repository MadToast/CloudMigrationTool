using CloudMigrationTool.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationSource.OneDrive
{
    internal class OneDriveCloudInfo : CloudInfo
    {
        public override LogoInfo Logos => throw new NotImplementedException();

        public override string ProviderName => "Microsoft OneDrive";

        public OneDriveCloudInfo(long totalSize, long remainingSize, long usedSize)
        {
            TotalSize = totalSize;
            RemainingSize = remainingSize;
            UsedSize = usedSize;
        }
    }
}
