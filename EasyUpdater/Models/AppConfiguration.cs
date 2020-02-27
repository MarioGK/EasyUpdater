using System;

namespace EasyUpdater.Models
{
    public class AppConfiguration
    {
        public string DownloadUrl { get; set; }

        public string VersionString { get; set; }

        internal Version Version => new Version(VersionString);
        //public string ChangeLogUrl { get; set; }
    }
}