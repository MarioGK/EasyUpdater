using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyUpdater.Helpers;
using EasyUpdater.Models;

namespace EasyUpdater.UpdateRoutines
{
    internal class DynamicUpdateRoutine : IUpdateRoutine
    {
        private readonly AppConfiguration appConfiguration;
        internal DynamicUpdateRoutine(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
        }

        public async Task Run()
        {
            var files = await FetchFiles();
        }

        public async Task<List<AppFile>> FetchFiles()
        {
            return await JsonFetcher.FetchAsync<List<AppFile>>(appConfiguration.DownloadUrl + "/files.json");
        }

        public bool Running { get; set; }
        public double CurrentProgress { get; set; }
        public double TotalFilesToDownload { get; set; }
        public double DownloadedFiles { get; set; }
    }
}