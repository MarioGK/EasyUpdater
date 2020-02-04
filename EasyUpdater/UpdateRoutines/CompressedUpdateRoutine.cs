using System.Threading.Tasks;
namespace EasyUpdater.UpdateRoutines
{
    public class CompressedUpdateRoutine : IUpdateRoutine
    {
        public async Task Run()
        {
            
        }

        public bool Running { get; set; }
        public double CurrentProgress { get; set; }
        public double TotalFilesToDownload { get; set; }
        public double DownloadedFiles { get; set; }
    }
}