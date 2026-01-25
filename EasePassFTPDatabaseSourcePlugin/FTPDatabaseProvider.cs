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

        public bool ExternalConfigEditingSupport => false;

        public IDatabaseSource[] GetDatabases()
        {
            return FTPConfigurationDatabase.LoadConfigurations().Select(config => new FTPDatabaseSource(config)).ToArray();
        }

        public string GetConfigurationJSON()
        {
            if (FTPConfigurationDatabase.HasConfigurationJSON())
                return FTPConfigurationDatabase.GetConfigurationJSON();
            else
                return GetSampleJsonConfig();
        }

        public bool SetConfigurationJSON(string configJson)
        {
            if (configJson == GetSampleJsonConfig())
                return true;
            if (!FTPConfigurationDatabase.ValidateConfigurationJSON(configJson))
                return false;
            return FTPConfigurationDatabase.SetConfigurationJSON(configJson);
        }

        public string GetSampleJsonConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            sb.AppendLine("  {");
            sb.AppendLine("    \"Hint\": \"Please adapt this sample configuration. All values are required. Make one entry per remote database\",");
            sb.AppendLine("    \"Mode\": \"FTP\",");
            sb.AppendLine("    \"Host\": \"ftp.example.com\",");
            sb.AppendLine("    \"Port\": 21,");
            sb.AppendLine("    \"Username\": \"your_username\",");
            sb.AppendLine("    \"Password\": \"your_password\",");
            sb.AppendLine("    \"RemotePath\": \"/path/to/your/database.epdb\"");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            return sb.ToString();
        }

        public void OpenExternalConfigEditor() { }
    }
}
