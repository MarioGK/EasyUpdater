using System;
using System.IO;
using System.Security.Cryptography;
using System.Timers;
using EasyUpdater.Enumerations;
using EasyUpdater.Helpers;

namespace EasyUpdater
{
    public class EasyUpdater : IDisposable
    {
        public bool Running { get; private set; }
        public double CurrentProgress { get; private set; }
        public double TotalFilesToDownload { get; private set; }
        public double DownloadedFiles { get; private set; }

        public TimeSpan UpdateCheckInterval { get; set; } = new TimeSpan(1,0,0);

        /// <summary>
        /// Path to save the downloads.
        /// </summary>
        public string DownloadPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
        
        public DownloadMode DownloadMode { get; set; } = DownloadMode.Dynamic;

        public HashAlgorithm HashAlgorithm { get; set; } = new Crc32();

        /// <summary>
        /// Path to extract the zip file if download is compressed.
        /// </summary>
        public string InstallationPath { get; set; }

        private AppConfiguration appConfiguration;

        private readonly string appConfigUrl;
        private readonly Timer timer;

        public EasyUpdater(string appConfigUrl)
        {
            this.appConfigUrl = appConfigUrl;
            timer = new Timer(new TimeSpan(1,0,0).TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, args) => CheckForUpdate();
        }

        public async void Start()
        {
            appConfiguration = await JsonFetcher.FetchAsync<AppConfiguration>(appConfigUrl);
            
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public async void CheckForUpdate()
        {
            
        }

        public void SetUpdateInterval(TimeSpan updateInterval)
        {
            timer.Interval = updateInterval.TotalMilliseconds;
        }

        public void Dispose()
        {
            timer?.Dispose();
            HashAlgorithm?.Dispose();
        }
    }
}