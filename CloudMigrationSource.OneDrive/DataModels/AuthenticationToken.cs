using CloudMigrationTool.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationSource.OneDrive.DataModels
{
    internal class AuthenticationToken
    {
        public DateTimeOffset ExpiresOn { get; set; }
        public string Token { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
