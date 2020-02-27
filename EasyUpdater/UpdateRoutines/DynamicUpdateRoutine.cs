using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EasyUpdater.Crypto;
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
            NotifyStart();

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
                        var downloadLocation = Path.Combine(AssemblyPath, differentFile.RelativeFilePath);
                        DownloadFile(url, downloadLocation);
                    }
                }
            }

            NotifyEnd();
        }

        /// <summary>
        ///     Returns the different files
        /// </summary>
        /// <returns></returns>
        private IEnumerable<AppFile> GetDifferentFiles(IEnumerable<AppFile> remoteFiles)
        {
            return from remoteFile in remoteFiles
                let localFile = Path.Combine(AssemblyPath, remoteFile.RelativeFilePath)
                let localFileChecksum = localFile.CalculateChecksum()
                where remoteFile.Checksum != localFileChecksum
                select remoteFile;
        }
        


        public async Task<List<AppFile>> FetchFiles()
        {
            return await JsonFetcher.FetchAsync<List<AppFile>>(appConfiguration.DownloadUrl + "/files.json");
        }
    }
}