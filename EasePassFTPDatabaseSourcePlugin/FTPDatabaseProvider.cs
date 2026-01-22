using EasePassExtensibility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    public class FTPDatabaseProvider : IDatabaseProvider
    {
        public string SourceName => "FTP Server";

        public Uri SourceIcon => Icon.GetIconUri();

        public IDatabaseSource[] GetDatabases()
        {
            return FTPConfigurationDatabase.LoadConfigurations().Select(config => new FTPDatabaseSource(config)).ToArray();
        }

        public string GetConfigurationJSON()
        {
            return FTPConfigurationDatabase.GetConfigurationJSON();
        }

        public bool SetConfigurationJSON(string configJson)
        {
            return FTPConfigurationDatabase.SetConfigurationJSON(configJson);
        }
    }
}
