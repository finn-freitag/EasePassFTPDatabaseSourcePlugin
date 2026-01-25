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

        public static bool HasConfigurationJSON()
        {
            if (ConfigurationStorage.Instance == null)
                return false;
            string config = ConfigurationStorage.Instance.LoadString("config");
            if (string.IsNullOrEmpty(config))
                return false;
            return true;
        }

        public static string GetConfigurationJSON()
        {
            string config = ConfigurationStorage.Instance.LoadString("config");
            if (string.IsNullOrEmpty(config))
                return "[]";
            return Obfuscator.Decrypt(config);
        }

        public static bool SetConfigurationJSON(string configJson)
        {
            ConfigurationStorage.Instance.SaveString("config", Obfuscator.Encrypt(configJson));
            return true;
        }

        public static bool ValidateConfigurationJSON(string configJson)
        {
            try
            {
                var configs = JsonSerializer.Deserialize<FTPConfig[]>(configJson, options);
                if (configs == null || configs.Length == 0)
                    return false;
                foreach (var config in configs)
                {
                    if (string.IsNullOrWhiteSpace(config.Host) ||
                        string.IsNullOrWhiteSpace(config.Username) ||
                        string.IsNullOrWhiteSpace(config.Password) ||
                        string.IsNullOrWhiteSpace(config.RemotePath) ||
                        config.Host == "ftp.example.com" ||
                        config.Password == "your_password" ||
                        config.Port <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
