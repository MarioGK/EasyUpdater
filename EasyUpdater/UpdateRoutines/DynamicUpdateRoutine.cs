using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyUpdater.Helpers;
using EasyUpdater.Models;

namespace EasyUpdater.UpdateRoutines
{
    internal class DynamicUpdateRoutine : BaseRoutine
    {
        private readonly AppConfiguration appConfiguration;
        
        internal DynamicUpdateRoutine(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
        }

        public override async Task Run()
        {
            if (!NotifyStart())
            {
                return;
            }

            var files = await FetchFiles();

            //If there are files to download
            if (files.Any())
            {
                var differentFiles = GetDifferentFiles(files).ToList();

                //If there are any different files
                if (differentFiles.Any())
                {
                    TotalFilesToDownload = differentFiles.Count;

                    foreach (var differentFile in differentFiles)
                    {
                        var url = appConfiguration.DownloadUrl + differentFile.FileName;
                        var downloadLocation = Path.Combine(AssemblyPath, differentFile.FilePath);
                        DownloadFile(url, downloadLocation);
                    }
                }
            }

            if (!NotifyEnd())
            {
                return;
            }
        }
        
        public async Task<List<AppFile>> FetchFiles()
        {
            return await JsonFetcher.FetchAsync<List<AppFile>>(appConfiguration.DownloadUrl + "/files.json");
        }
    }
}