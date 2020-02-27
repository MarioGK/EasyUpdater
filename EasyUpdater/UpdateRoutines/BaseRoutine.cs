using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyUpdater.UpdateRoutines
{
    public abstract class BaseRoutine : IDisposable
    {
        public event EasyUpdater.VoidDelegate ProgressChanged;
        public event EasyUpdater.VoidDelegate Started;
        public event EasyUpdater.VoidDelegate Finished;
        
        protected static readonly string AssemblyPath = Assembly.GetExecutingAssembly().Location;

        public bool Running { get; set; }
        public double CurrentProgress { get; set; }
        public double TotalFilesToDownload { get; set; }
        public double DownloadedFiles { get; set; }

        public bool CanRun { get; set; } = true;

        public abstract Task Run();


        protected void NotifyProgressChange(double progress)
        {
            CurrentProgress = progress;
            ProgressChanged?.Invoke();
        }

        protected void NotifyStart()
        {
            Started?.Invoke();
            Running = true;
        }

        protected void NotifyEnd()
        {
            Running = false;
            Finished?.Invoke();
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

        public void Dispose()
        {
        }
    }
}