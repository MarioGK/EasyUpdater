using System.IO;
using System.Text.Json.Serialization;
using EasyUpdater.Helpers;
using Newtonsoft.Json;

namespace EasyUpdater.Models
{
    public class AppFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Checksum { get; set; }

        [JsonConstructor]
        public AppFile()
        {
            
        }

        public AppFile(string absoluteFilePath)
        {
            FileName = Path.GetFileName(absoluteFilePath);
            FilePath = absoluteFilePath.RelativePath(EasyUpdater.CurrentDirectory);
            Checksum = absoluteFilePath.CalculateChecksum();
        }
    }
}