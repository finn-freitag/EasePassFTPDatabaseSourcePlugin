using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasePassFTPDatabaseSourcePlugin
{
    internal static class FTPConfigurationDatabase
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            // This converts the Enum to "SFTP" instead of 1
            Converters = { new JsonStringEnumConverter() },
            // Allows accessing internal classes
            IncludeFields = true
        };

        public static FTPConfig[] LoadConfigurations()
        {
            return JsonSerializer.Deserialize<FTPConfig[]>(GetConfigurationJSON(), options) ?? Array.Empty<FTPConfig>();
        }

        public static bool SaveConfigurations(FTPConfig[] configs)
        {
            try
            {
                return SetConfigurationJSON(JsonSerializer.Serialize<FTPConfig[]>(configs, options));
            }
            catch
            {
                return false;
            }
        }

        public static string GetConfigurationJSON()
        {
            string p = GetFilePath();
            if (File.Exists(p))
                return Obfuscator.Decrypt(File.ReadAllText(p));
            return "[]";
        }

        public static bool SetConfigurationJSON(string configJson)
        {
            try
            {
                File.WriteAllText(GetFilePath(), Obfuscator.Encrypt(configJson));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetFilePath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string pluginFolder = Path.Combine(appData, "EasePass", "Plugins", "EasePassFTPRemoteDatabasePlugin");
            if (!Directory.Exists(pluginFolder))
            {
                Directory.CreateDirectory(pluginFolder);
            }
            return Path.Combine(pluginFolder, "ftp_configurations.json");
        }
    }
}
