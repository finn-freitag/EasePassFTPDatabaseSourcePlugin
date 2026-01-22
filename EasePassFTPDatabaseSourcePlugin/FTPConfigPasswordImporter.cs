using EasePassExtensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    public class FTPConfigPasswordImporter : IPasswordImporter
    {
        public string SourceName => "Remote Database FTP Config";

        public Uri SourceIcon => Icon.GetIconUri();

        public PasswordItem[] ImportPasswords()
        {
            return FTPConfigurationDatabase.LoadConfigurations().Select(c=>new PasswordItem()
            {
                DisplayName = c.Mode.ToString() + ": " + c.Host,
                UserName = c.Username,
                Password = c.Password,
                Notes = $"Port: {c.Port}\nRemote Path: {c.RemotePath}\nMode: {c.Mode}"
            }).ToArray();
        }

        public bool PasswordsAvailable()
        {
            return FTPConfigurationDatabase.LoadConfigurations().Length > 0;
        }
    }
}
