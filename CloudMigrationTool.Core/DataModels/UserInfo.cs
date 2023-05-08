using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public abstract class UserInfo
    {
        /// <summary>
        /// Contains the unique identification of the user
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Contains the username
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Contains the Display Name for the user
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Contains the email for the user
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Contains the path to the file that contains the user's photo
        /// </summary>
        public string PhotoFile { get; }
    }
}
