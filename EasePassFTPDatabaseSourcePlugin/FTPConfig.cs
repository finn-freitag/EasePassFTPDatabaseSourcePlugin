using System;
using System.Collections.Generic;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    internal class FTPConfig
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 21;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string RemotePath { get; set; } = "";
        public FTPMode Mode { get; set; } = FTPMode.FTP;

        internal string GetUriString()
        {
            return $"{Mode.ToString().ToLower()}://{Host}:{Port}{RemotePath}";
        }

        internal string GetFilenameWithoutExt()
        {
            // use string methods, because OS is unclear
            string filename = RemotePath;
            int lastSlash = filename.LastIndexOf('/');
            if (lastSlash >= 0 && lastSlash < filename.Length - 1)
            {
                filename = filename.Substring(lastSlash + 1);
            }
            int lastDot = filename.LastIndexOf('.');
            if (lastDot > 0)
            {
                filename = filename.Substring(0, lastDot);
            }
            return filename;
        }

        public enum FTPMode
        {
            FTP,
            SFTP,
            FTPS
        }
    }
}
