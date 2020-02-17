using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using EasyUpdater.Helpers;
using EasyUpdater.Models;

namespace EasyUpdater.UpdateRoutines
{
    public abstract class BaseRoutine : IUpdateRoutine
    {
        protected static readonly string AssemblyPath = Assembly.GetExecutingAssembly().Location;
        
        public bool Running { get; set; }
        public double CurrentProgress { get; set; }
        public double TotalFilesToDownload { get; set; }
        public double DownloadedFiles { get; set; }
        
        public delegate void VoidDelegate();
        public delegate bool BoolDelegate();

        public event VoidDelegate ProgressChanged;
        public event BoolDelegate Start;
        public event BoolDelegate Finish;

        public abstract Task Run();

        protected void NotifyProgressChange(double progress)
        {
            CurrentProgress = progress;
            ProgressChanged?.Invoke();
        }
        
        protected bool NotifyStart()
        {
            var value = Start?.Invoke() ?? true;
            Running = value;
            return value;
        }

        protected bool NotifyEnd()
        {
            Running = false;
            return Finish?.Invoke() ?? true;
        }

        /// <summary>
        /// Returns the different files
        /// </summary>
        /// <returns></returns>
        protected static IEnumerable<AppFile> GetDifferentFiles(IEnumerable<AppFile> remoteFiles)
        {
            return from remoteFile in remoteFiles
                let localFile = Path.Combine(AssemblyPath, remoteFile.FilePath)
                let localFileChecksum = localFile.CalculateChecksum()
                where remoteFile.Checksum != localFileChecksum
                select remoteFile;
        }

        protected void DownloadFile(string url, string downloadLocation)
        {
            using var wc = new WebClient();
            wc.DownloadProgressChanged += (sender, args) => NotifyProgressChange(args.ProgressPercentage / 100d);
            wc.DownloadFileCompleted += (sender, args) =>
            {
                ++DownloadedFiles;
                NotifyProgressChange(1);
            };
            wc.DownloadFileAsync(new Uri(url), downloadLocation);
        }
    }
}