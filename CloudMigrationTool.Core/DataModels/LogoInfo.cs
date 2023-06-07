using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public class LogoInfo
    {
        /// <summary>
        /// Contains the light logo binary data to display in the app
        /// </summary>
        byte[] LightPhotoBinary { get; set; }

        /// <summary>
        /// Informs on the content type so the tool knows how to deal with the binary content above
        /// </summary>
        string LightPhotoContentType { get; set; }

        /// <summary>
        /// Contains the light logo binary data to display in the app
        /// </summary>
        byte[] DarkPhotoBinary { get; set; }

        /// <summary>
        /// Informs on the content type so the tool knows how to deal with the binary content above
        /// </summary>
        string DarkPhotoContentType { get; set; }
    }
}
