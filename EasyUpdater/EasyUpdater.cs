using System;
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
        internal static string CurrentDirectory;

        private readonly string appConfigUrl;
        private readonly Timer timer;
        public BaseRoutine UpdateRoutine { get; set; }
        
        public delegate void VoidDelegate();
        
        public event VoidDelegate ProgressChanged;
        public event VoidDelegate Started;
        public event VoidDelegate Finished;

        private AppConfiguration appConfiguration;
        private TimeSpan updateCheckInterval = new TimeSpan(1, 0, 0);

        public EasyUpdater(string appConfigUrl)
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            this.appConfigUrl = appConfigUrl;
            
            timer = new Timer(new TimeSpan(1, 0, 0).TotalMilliseconds)
            {
                AutoReset = true
            };
        }

        private bool initialized;

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            timer.Elapsed += (sender, args) => CheckForUpdate();

            UpdateRoutine = DownloadMode switch
            {
                DownloadMode.Dynamic => new DynamicUpdateRoutine(appConfiguration),
                DownloadMode.Compressed => new CompressedUpdateRoutine(),
                //DownloadMode.Custom => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            UpdateRoutine.Started += NotifyStartEvent;
            UpdateRoutine.Finished += NotifyFinishEvent;
            UpdateRoutine.ProgressChanged += NotifyProgressChanged;
        }

        public bool CanRun
        {
            get => UpdateRoutine.CanRun;
            set => UpdateRoutine.CanRun = value;
        }
        public bool Running => UpdateRoutine.Running;
        public double CurrentProgress => UpdateRoutine.CurrentProgress;
        public double TotalFilesToDownload => UpdateRoutine.TotalFilesToDownload;
        public double DownloadedFiles => UpdateRoutine.DownloadedFiles;

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
        ///     Path to save the downloads.
        /// </summary>
        public string DownloadPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

        public DownloadMode DownloadMode { get; set; } = DownloadMode.Dynamic;
        
        /// <summary>
        ///     Path to extract the zip file if download is compressed.
        /// </summary>
        public string InstallationPath { get; set; }

        public void Dispose()
        {
            timer?.Dispose();
            UpdateRoutine?.Dispose();
        }

        /// <summary>
        /// Starts and initializes the Updater.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public async void Start()
        {
            Initialize();
            
            if (DownloadMode == DownloadMode.Custom && UpdateRoutine == null)
            {
                throw new Exception("You have to provide a UpdateRoutine when you select the Custom download mode.");
            }
            
            appConfiguration = await JsonFetcher.FetchAsync<AppConfiguration>(appConfigUrl);

            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Force a check for updates.
        /// </summary>
        public async void CheckForUpdate()
        {
            if (Running)
            {
                return;
            }
            
            await UpdateRoutine.Run();
        }

        private void NotifyStartEvent()
        {
            Started?.Invoke();
        }
        
        private void NotifyFinishEvent()
        {
            Finished?.Invoke();
        }

        private void NotifyProgressChanged()
        {
            ProgressChanged?.Invoke();
        }
    }
}