using CloudMigrationTool.Core.DataModels;

namespace CloudMigrationTool.Core.Interfaces
{
    public interface ISourceInfo
    {
        /// <summary>
        /// Class containing logos to be shown in the application
        /// </summary>
        LogoInfo LogoInfo { get; }

        /// <summary>
        /// This field should contain the brand name
        /// </summary>
        string CloudName { get; }

        /// <summary>
        /// This field contains the company providing the cloud
        /// </summary>
        string CloudProvider { get; }

        /// <summary>
        /// Informs the app if this cloud provider working in console mode. (Authentication, etc)
        /// </summary>
        bool CanUseConsoleMode { get; }
    }
}
