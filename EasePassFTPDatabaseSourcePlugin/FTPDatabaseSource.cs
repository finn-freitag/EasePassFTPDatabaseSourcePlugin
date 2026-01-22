using EasePassExtensibility;
using FluentFTP;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    public class FTPDatabaseSource : IDatabaseSource
    {
        private FTPConfig config;

        public string DatabaseName => config.GetFilenameWithoutExt();

        public string SourceDescription => config.GetUriString();

        internal FTPDatabaseSource(FTPConfig config)
        {
            this.config = config;
        }

        public IDatabaseSource.DatabaseAvailability GetAvailability()
        {
            // unable to handle login sessions
            return IDatabaseSource.DatabaseAvailability.Available;
        }

        public DateTime GetLastTimeModified()
        {
            return DateTime.MinValue;
        }

        public void Login() { /* unable to handle login sessions */ }

        public void Logout() { /* unable to handle login sessions */ }

        public byte[] GetDatabaseFileBytes()
        {
            if(config.Mode == FTPConfig.FTPMode.SFTP)
            {
                using var client = new SftpClient(config.Host, config.Port, config.Username, config.Password);
                client.Connect();
                MemoryStream ms = new MemoryStream();
                client.DownloadFile(config.RemotePath, ms);
                client.Disconnect();
                client.Dispose();
                return ms.ToArray();
            }
            else
            {
                using var client = new FtpClient(config.Host, config.Username, config.Password, config.Port);
                client.AutoConnect();

                byte[] outBytes;
                bool res = client.DownloadBytes(out outBytes, config.RemotePath);
                client.Disconnect();
                client.Dispose();
                if (res)
                    return outBytes;
                else
                    return Array.Empty<byte>();
            }
        }

        public bool SaveDatabaseFileBytes(byte[] databaseFileBytes)
        {
            if (config.Mode == FTPConfig.FTPMode.SFTP)
            {
                using var client = new SftpClient(config.Host, config.Port, config.Username, config.Password);
                client.Connect();
                using var ms = new MemoryStream(databaseFileBytes);
                client.UploadFile(ms, config.RemotePath);
                client.Disconnect();
                client.Dispose();
                return true;
            }
            else
            {
                using var client = new FtpClient(config.Host, config.Username, config.Password, config.Port);
                client.AutoConnect();
                var res = client.UploadBytes(databaseFileBytes, config.RemotePath);
                client.Disconnect();
                client.Dispose();
                return res == FtpStatus.Success;
            }
        }
    }
}
