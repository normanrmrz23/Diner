namespace Diner.Services
{
    public partial class FileServicePaths : IFileServicePaths
    {
        private const string _appDataFilesPath = "Android/data/com.norman.diner/files";

        public string PublicStoragePath { get; private set; }
        public string PrivateStoragePath { get; private set; }

        public FileServicePaths()
        {
            // On Android, the "user" storage is the external storage. "Private" storage
            // goes to the app's "internal storage" directory.
            PublicStoragePath = GetExternalStorageRoot();
            PrivateStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private static string GetExternalStorageRoot()
        {
            var appContext = Android.App.Application.Context;
            var externalPath = appContext.GetExternalFilesDir(null);
            if (externalPath == null)
                return string.Empty;

            return externalPath.Path.Replace(_appDataFilesPath, string.Empty).TrimEnd('/');
        }
    }
}
