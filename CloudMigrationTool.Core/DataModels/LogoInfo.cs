using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public class LogoInfo
    {
        /// <summary>
        /// Contains a light logo to be displayed in the app
        /// </summary>
        string LightPhotoFile { get; set; }

        /// <summary>
        /// Contains a dark logo to be displayed in the app
        /// </summary>
        string DarkPhotoFile { get; set; }
    }
}
