using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public abstract class LogoInfo
    {
        /// <summary>
        /// Contains the light logo data to display in the app
        /// 
        /// If null, it means the cloud source does not have logo content
        /// </summary>
        public abstract byte[] LightPhotoContent();

        /// <summary>
        /// Contains the light logo svg content to display in the app
        /// 
        /// If null, it means the cloud source does not have svg logo content
        /// </summary>
        public abstract string LightPhotoSvgContent();

        /// <summary>
        /// Contains the dark logo binary data to display in the app
        /// 
        /// If null, it means the cloud source does not have logo content
        /// </summary>
        public abstract byte[] DarkPhotoContent();

        /// <summary>
        /// Contains the dark logo binary data to display in the app
        /// 
        /// If null, it means the cloud source does not have svg logo content
        /// </summary>
        public abstract string DarkPhotoSvgContent();
    }
}
