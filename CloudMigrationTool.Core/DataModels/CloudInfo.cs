using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public class CloudInfo
    {
        /// <summary>
        /// Contains the Id of the root Cloud
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Contains the Name of the Drive or Cloud Provider
        /// </summary>
        public string Name { get; protected set; }

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

        public CloudInfo(string id, string name, long totalSize, long remainingSize, long usedSize)
        {
            Id = id;
            Name = Name;
            TotalSize = totalSize;
            RemainingSize = remainingSize;
            UsedSize = usedSize;
        }
    }
}
