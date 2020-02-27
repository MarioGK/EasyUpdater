using CommandLine;

namespace EasyUpdater.Publisher
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('m', "mode", Required = false, HelpText = "Set the publish mode.")]
        public PublishMode PublishMode { get; set; } = PublishMode.GenerateOnly;
        
        [Option('p', "path", Required = true, HelpText = "Folder path.")]
        public string Path { get; set; }
        
        [Option('o', "output", Required = false, HelpText = "Output folder path.")]
        public string OutputPath { get; set; }
    }

}