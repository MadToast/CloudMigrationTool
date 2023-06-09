﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ICloudItem
    {

        #region Properties
        /// <summary>
        /// Returns a dictionary of the attributes where the key is the attribute name and value is the value of the attribute
        /// </summary>
        IReadOnlyDictionary<string, object> Attributes { get; }

        /// <summary>
        /// Returns the Creation Time
        /// </summary>
        DateTimeOffset? CreationDateTime { get; }

        /// <summary>
        /// Returns the Creation Time in UTC
        /// </summary>
        DateTimeOffset? LastModifiedDateTime { get; }

        /// <summary>
        /// Returns the full path of the item
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Returns the name of the item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the length of the item in bytes
        /// </summary>
        long Length { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Sets the attributes for this Cloud item.
        /// </summary>
        /// <param name="attributes"></param>
        Task<bool> SetAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Gets the parent of this Cloud item
        /// </summary>
        /// <returns></returns>
        Task<ICloudDirectory> GetParent();
        #endregion
    }
}
