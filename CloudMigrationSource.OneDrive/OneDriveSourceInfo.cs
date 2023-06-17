using CloudMigrationSource.OneDrive.Constants;
using CloudMigrationTool.Core.DataModels;
using CloudMigrationTool.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationSource.OneDrive
{
    public class OneDriveSourceInfo : ISourceInfo
    {
        public LogoInfo LogoInfo => OneDrive.Constants.OneDriveLogoInfo.Instance;

        public string CloudName => "OneDrive";

        public string CloudProvider => "Microsoft Corporation";

        public bool CanUseConsoleMode => true;
    }
}
