using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationSource.OneDrive
{
    internal static class Constants
    {
        internal static string ApplicationId = "23b92609-1d31-4990-8fc5-d74151d34776";

        internal static string PathPrefix = "/drive/root:";

        internal static string[] Scopes =
        {
            "https://graph.microsoft.com/.default"
        };

        internal static string[] AllowedImageTypes =
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/tiff"
        };

        internal static IReadOnlyDictionary<string, string> ImageTypeExtensionMappping = new Dictionary<string, string>()
        {
            { "image/jpg", ".jpg" },
            { "image/jpeg", ".jpg" },
            { "image/png", ".png" },
            { "image/gif", ".gif" },
            { "image/tiff", ".tiff" },
        };
    }
}
