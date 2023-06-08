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
        public abstract string ProviderName { get; }

        /// <summary>
        /// Contains the Total Space of the Cloud drive in bytes
        /// </summary>
        public long TotalSize { get; protected set; }

        /// <summary>
        /// Contains the Used Space of the Cloud drive in bytes
        /// </summary>
        public long UsedSize { get; protected set; }

        /// <summary>
        /// Contains the Remaining Available Space of the Cloud drive in bytes
        /// </summary>
        public long RemainingSize { get; protected set; }
    }
}
