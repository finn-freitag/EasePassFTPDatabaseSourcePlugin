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

        public string DatabaseName => config.GetFilenameWithoutExt() + " (" + config.Mode.ToString() + ")";

        public string SourceDescription => config.GetUriString();

        public bool IsReadOnly => false;

        public IDatabaseSource.DatabaseAvailability Availability
        {
            get
            {
                // unable to handle login sessions
                return IDatabaseSource.DatabaseAvailability.Available;
            }
        }

        public DateTime LastTimeModified
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        public Action OnPropertyChanged { get; set; }

        internal FTPDatabaseSource(FTPConfig config)
        {
            this.config = config;
        }

        public void Login() { /* unable to handle login sessions */ }

        public void Logout() { /* unable to handle login sessions */ }

        public Task<byte[]> GetDatabaseFileBytes()
        {
            if(config.Mode == FTPConfig.FTPMode.SFTP)
            {
                using var client = new SftpClient(config.Host, config.Port, config.Username, config.Password);
                client.Connect();
                MemoryStream ms = new MemoryStream();
                client.DownloadFile(config.RemotePath, ms);
                client.Disconnect();
                client.Dispose();
                return Task.FromResult(ms.ToArray());
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
                    return Task.FromResult(outBytes);
                else
                    return Task.FromResult(Array.Empty<byte>());
            }
        }

        public Task<bool> SaveDatabaseFileBytes(byte[] databaseFileBytes)
        {
            if (config.Mode == FTPConfig.FTPMode.SFTP)
            {
                using var client = new SftpClient(config.Host, config.Port, config.Username, config.Password);
                client.Connect();
                using var ms = new MemoryStream(databaseFileBytes);
                client.UploadFile(ms, config.RemotePath);
                client.Disconnect();
                client.Dispose();
                return Task.FromResult(true);
            }
            else
            {
                using var client = new FtpClient(config.Host, config.Username, config.Password, config.Port);
                client.AutoConnect();
                var res = client.UploadBytes(databaseFileBytes, config.RemotePath);
                client.Disconnect();
                client.Dispose();
                return Task.FromResult(res == FtpStatus.Success);
            }
        }
    }
}
