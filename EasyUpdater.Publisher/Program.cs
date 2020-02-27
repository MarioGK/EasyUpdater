using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using CommandLine;
using EasyUpdater.Models;
using Newtonsoft.Json;

namespace EasyUpdater.Publisher
{
    internal static class Program
    {
        private static Options _options;
        
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => _options = o);

            if (_options == null)
            {
                return;
            }

            switch (_options.PublishMode)
            {
                case PublishMode.GenerateOnly:
                    GenerateOnly();
                    break;
                case PublishMode.Local:
                    Local();
                    break;
                case PublishMode.SSH:
                    break;
                case PublishMode.SFTP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.Read();
        }

        public static List<AppFile> GetAppFiles()
        {
            return Directory.EnumerateFiles(_options.Path, "*.*", SearchOption.AllDirectories).Select(x => new AppFile(x)).ToList();
        }

        public static void GenerateJsonFile(IEnumerable<AppFile> files)
        {
            Console.WriteLine("Generating json files ...");

            File.WriteAllText(Path.Combine(_options.Path, "files.json"), JsonConvert.SerializeObject(files));
            
            Console.WriteLine("Generated json files!");
        }
        
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static void StartTimer(string message)
        {
            Console.WriteLine($"Started to time ({message})...");
            Stopwatch.Start();
        }

        private static void StopTimer(string message)
        {
            Console.WriteLine($"{message} Took ${Stopwatch.ElapsedMilliseconds} MS");
            
            Stopwatch.Reset();
            Stopwatch.Stop();
        }

        public static void GenerateOnly()
        {
            StartTimer("Getting app files");
            var files = GetAppFiles();
            StopTimer("Getting app files");
            
            StartTimer("Generating json files");
            GenerateJsonFile(files);
            StopTimer("Generating json files");
        }

        public static void Local()
        {
            StartTimer("Getting app files");
            var files = GetAppFiles();
            StopTimer("Getting app files");
            
            StartTimer("Generating json files");
            GenerateJsonFile(files);
            StopTimer("Generating json files");

            StartTimer("Copying files");
            foreach (var file in files)
            {
                File.Copy(file.AbsoluteFilePath, Path.Combine(_options.OutputPath, file.RelativeFilePath), true);
            }
            StopTimer("Copying files");
        }
    }
}