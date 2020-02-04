using System.Threading.Tasks;

namespace EasyUpdater.UpdateRoutines
{
    internal interface IUpdateRoutine
    {
        internal Task Run();

        bool Running { get; set; }
        double CurrentProgress{ get; set; }
        double TotalFilesToDownload{ get; set; }
        double DownloadedFiles{ get; set; }
    }
}