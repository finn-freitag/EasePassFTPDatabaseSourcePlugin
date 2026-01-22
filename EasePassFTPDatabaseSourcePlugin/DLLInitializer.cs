using EasePassExtensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    public class DLLInitializer : IInitializer
    {
        public void Init()
        {
            string path = Path.GetDirectoryName(this.GetType().Assembly.Location);

            WriteAndFlush(Path.Combine(path, "BouncyCastle.Cryptography.dll"), Properties.Resources.BouncyCastle_Cryptography);
            WriteAndFlush(Path.Combine(path, "FluentFTP.dll"), Properties.Resources.FluentFTP);
            WriteAndFlush(Path.Combine(path, "Microsoft.Extensions.DependencyInjection.Abstractions.dll"), Properties.Resources.Microsoft_Extensions_DependencyInjection_Abstractions);
            WriteAndFlush(Path.Combine(path, "Microsoft.Extensions.Logging.Abstractions.dll"), Properties.Resources.Microsoft_Extensions_Logging_Abstractions);
            WriteAndFlush(Path.Combine(path, "Renci.SshNet.dll"), Properties.Resources.Renci_SshNet);
        }

        private void WriteAndFlush(string filePath, byte[] data)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush(true);
            }
        }
    }
}
