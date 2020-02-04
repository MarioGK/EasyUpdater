using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Timers;
using EasyUpdater.Crypto;
using EasyUpdater.Enumerations;
using EasyUpdater.Helpers;
using EasyUpdater.Models;
using EasyUpdater.UpdateRoutines;

namespace EasyUpdater
{
    public class EasyUpdater : IDisposable
    {
        public bool Running => updateRoutine.Running;
        public double CurrentProgress  => updateRoutine.CurrentProgress;
        public double TotalFilesToDownload  => updateRoutine.TotalFilesToDownload;
        public double DownloadedFiles => updateRoutine.DownloadedFiles;

        public TimeSpan UpdateCheckInterval
        {
            get => updateCheckInterval;
            set
            {
                updateCheckInterval = value;
                timer.Interval = updateCheckInterval.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Path to save the downloads.
        /// </summary>
        public string DownloadPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
        
        public DownloadMode DownloadMode { get; set; } = DownloadMode.Dynamic;

        public static HashAlgorithm HashAlgorithm { get; set; } = new Crc32();

        /// <summary>
        /// Path to extract the zip file if download is compressed.
        /// </summary>
        public string InstallationPath { get; set; }

        private AppConfiguration appConfiguration;

        private readonly string appConfigUrl;
        private readonly Timer timer;
        private TimeSpan updateCheckInterval = new TimeSpan(1, 0, 0);
        private readonly IUpdateRoutine updateRoutine;

        internal static string CurrentDirectory;

        public EasyUpdater(string appConfigUrl)
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            this.appConfigUrl = appConfigUrl;
            timer = new Timer(new TimeSpan(1,0,0).TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, args) => CheckForUpdate();

            updateRoutine = DownloadMode switch
            {
                DownloadMode.Dynamic => new DynamicUpdateRoutine(appConfiguration),
                DownloadMode.Compressed => new CompressedUpdateRoutine(),
                _ => throw new ArgumentOutOfRangeException()
            };
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
            await updateRoutine.Run();
        }

        public void Dispose()
        {
            timer?.Dispose();
            HashAlgorithm?.Dispose();
        }
    }
}