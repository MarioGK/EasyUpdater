using System.IO;
using EasyUpdater.Helpers;
using Newtonsoft.Json;

namespace EasyUpdater.Models
{
    public class AppFile
    {
        [JsonConstructor]
        public AppFile()
        {
        }
        
        public AppFile(string absoluteFilePath)
        {
            FileName = Path.GetFileName(absoluteFilePath);
            AbsoluteFilePath = absoluteFilePath;
            RelativeFilePath = absoluteFilePath.RelativePath(Path.GetDirectoryName(absoluteFilePath));
            Checksum = absoluteFilePath.CalculateChecksum();
        }

        public string FileName { get; set; }
        [JsonIgnore]
        public string AbsoluteFilePath { get; set; }
        public string RelativeFilePath { get; set; }
        public string Checksum { get; set; }
    }
}