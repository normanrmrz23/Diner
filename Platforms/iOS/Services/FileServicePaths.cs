namespace Diner.Services
{
    public partial class FileServicePaths : IFileServicePaths
    {
        public string PublicStoragePath { get; private set; }

        public string PrivateStoragePath { get; private set; }

        public FileServicePaths()
        {
            // IOS doesn't have an "easy" concept of public files, so these values
            // don't have as great an explanation as AndroidFileService.
            PublicStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // [MOB-979] on iOS this (Resources) points to /Library
            PrivateStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
        }
    }
}
