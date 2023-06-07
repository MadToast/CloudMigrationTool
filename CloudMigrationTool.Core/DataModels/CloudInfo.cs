using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public abstract class CloudInfo
    {
        /// <summary>
        /// Contains the logos for this cloud provider
        /// </summary>
        public abstract LogoInfo Logos { get; }

        /// <summary>
        /// Contains the name of the provider of the cloud
        /// </summary>
        public string ProviderName { get; private set; }

        /// <summary>
        /// Contains the Total Space of the Cloud drive in bytes
        /// </summary>
        public long TotalSize { get; private set; }

        /// <summary>
        /// Contains the Used Space of the Cloud drive in bytes
        /// </summary>
        public long UsedSize { get; private set; }

        /// <summary>
        /// Contains the Remaining Available Space of the Cloud drive in bytes
        /// </summary>
        public long RemainingSize { get; private set; }
    }
}
