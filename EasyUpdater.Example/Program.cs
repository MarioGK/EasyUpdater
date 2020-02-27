using System;
using System.Threading;

namespace EasyUpdater.Example
{
    internal static class Program
    {
        private static readonly EasyUpdater Updater = new EasyUpdater("url");
        private static void Main(string[] args)
        {
            Updater.ProgressChanged += UpdateTitle;
            Updater.Finished += () =>
            {
                Console.WriteLine("Finished!");
            };

            Console.WriteLine("Starting update check timer ...");
            Updater.Start();
            
            Console.WriteLine("Forcing a update check");
            Updater.CheckForUpdate();

            Console.Read();
        }

        private static void UpdateTitle()
        {
            Console.Title = $"Running:{Updater.Running} | Progress:{Updater.CurrentProgress*100}% | {Updater.DownloadedFiles}/{Updater.TotalFilesToDownload}";
        }
    }
}