namespace EasyUpdater.Enumerations
{
    public enum DownloadMode
    {
        /// <summary>
        /// It updates only the file that have changed.
        /// </summary>
        Dynamic,
        /// <summary>
        /// It downloads the whole application and replaces the old one with the new one.
        /// </summary>
        Compressed
    }
}