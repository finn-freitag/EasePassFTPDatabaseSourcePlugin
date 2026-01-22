using EasePassExtensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasePassFTPDatabaseSourcePlugin
{
    public class AboutPage : IAboutPlugin
    {
        public string PluginName => "FTP Remote Database";

        public string PluginDescription => "This plugin can syncronize your passwords over multiple devices via FTP/FTPS/SFTP.";

        public string PluginAuthor => "Finn Freitag";

        public string PluginAuthorURL => "https://finnfreitag.com?ref=ep_plgn_ftprmtdb";

        public Uri PluginIcon => Icon.GetIconUri();
    }
}
