using CloudMigrationTool.Core.DataModels;
using Microsoft.Graph.Education.Classes.Item.Assignments.Item.Submissions.Item.Return;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigrationSource.OneDrive.Constants
{
    public class OneDriveLogoInfo : LogoInfo
    {
        public static OneDriveLogoInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OneDriveLogoInfo();
                }

                return _instance;
            }
        }

        private static OneDriveLogoInfo _instance;
        private string _oneDriveLogoContent;

        private OneDriveLogoInfo() {}

        public override byte[] DarkPhotoContent() => null;

        public override string DarkPhotoSvgContent() => GetOneDriveLogo();

        public override byte[] LightPhotoContent() => null;

        public override string LightPhotoSvgContent() => GetOneDriveLogo();

        private string GetOneDriveLogo()
        {
            if (_oneDriveLogoContent == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("OneDrive_Icon"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    _oneDriveLogoContent = reader.ReadToEnd();
                }
            }

            return _oneDriveLogoContent;
        }
    }
}
