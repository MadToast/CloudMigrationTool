using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.DataModels
{
    public class UserInfo
    {
        public UserInfo(string id, string name, string displayName, string email, string photoFile) {
            Id = id;
            Name = name;
            DisplayName = displayName;
            Email = email;
            PhotoFile = photoFile;
        }

        /// <summary>
        /// Contains the unique identification of the user
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Contains the username
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Contains the Display Name for the user
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Contains the email for the user
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Contains the path to the file that contains the user's photo
        /// </summary>
        public string PhotoFile { get; private set; }
    }
}
