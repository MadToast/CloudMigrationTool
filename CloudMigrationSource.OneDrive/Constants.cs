using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationSource.OneDrive
{
    internal static class Constants
    {
        public static string ApplicationId = "23b92609-1d31-4990-8fc5-d74151d34776";

        public static string[] Scopes =
        {
            "https://graph.microsoft.com/.default"
        };

        public static string[] AllowedImageTypes =
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/tiff"
        };

        public static IReadOnlyDictionary<string, string> ImageTypeExtensionMappping = new Dictionary<string, string>()
        {
            { "image/jpg", ".jpg" },
            { "image/jpeg", ".jpg" },
            { "image/png", ".png" },
            { "image/gif", ".gif" },
            { "image/tiff", ".tiff" },
        };
    }
}
