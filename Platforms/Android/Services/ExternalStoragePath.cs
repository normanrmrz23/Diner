#nullable enable
namespace Diner.Services
{
    internal partial class ExternalStoragePath
    {
        // Logging would be nice, unfortunately bootstrap classes use this
        // class even before IoC has initialized.

        // Exceptions are left to bubble up, mainly because of inability to log
        // them.

        // This type is a singleton due to Xamarin DependencyService mechanisms,
        // so don't cache values that might change.

        public string LocalApplicationData => GetLocalApplicationData();

        public string ExternalStorageRoot => GetExternalStorageRoot();

        private static string GetExternalStorageRoot()
        {
            var appContext = Android.App.Application.Context;
            var externalPath = appContext.GetExternalFilesDir(null);
            if (externalPath == null)
                return string.Empty;

            var externalStorageRoot = externalPath.Path.Replace(_appDataFilesPath, string.Empty).TrimEnd('/');

            var fileSystemRoot = FileSystem.AppDataDirectory;

            return externalStorageRoot;
        }

        // BSL - NOTE this was changed for CP2/Maui
        private const string _appDataFilesPath = "Android/data/com.norman.diner/files";

        public string FilesFolder => _appDataFilesPath;

        private readonly string _applicationDataPath;
        public string ApplicationData => _applicationDataPath;

        // Not cached because ejecting/mounting cards can change the status, and this type
        // is a singleton due to how DependencyService works.
        public string? BackupPathBase => GetBackupPathBase();


        public ExternalStoragePath()
        {
            //_applicationDataPath = FileSystem.AppDataDirectory;
            _applicationDataPath = GetApplicationDataPath();
        }

        private string GetApplicationDataPath()
        {
            var appContext = Android.App.Application.Context;

            // [MOB-77] - Generally the device's internal storage is the first
            // "external" storage directory. 
            var externalPath = appContext.GetExternalFilesDir(null);

            if (externalPath != null)
            {
                var fullPath = externalPath.AbsolutePath;
                var legacyPath = GetLegacyApplicationDataPath(fullPath);

                var fileSystemRoot = FileSystem.AppDataDirectory;

                var appDataPath = Path.Combine(legacyPath, FilesFolder);
                return appDataPath;
            }
            else
            {
                return "";
            }
        }

        private static string GetLocalApplicationData()
        {
            //return FileSystem.AppDataDirectory;
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return localAppDataPath;
        }

        // [MOB-77] Hack: Current code expects to get the part *before*
        // Android/data/com.aiworldwide.aim in the path, but Android API returns the part
        // *including* that.
        private static string GetLegacyApplicationDataPath(string fullPath)
        {
            var newPartIndex = fullPath.IndexOf("Android");
            if (newPartIndex != -1)
            {
                return fullPath.Substring(0, newPartIndex);
            }
            else
            {
                return fullPath;
            }
        }

        private static string? GetBackupPathBase()
        {
            try
            {
                var appContext = Android.App.Application.Context;

                // [MOB-77] - Generally, what we consider internal flash is an "external storage"
                // path. Usually it's the first element in this array. If there is a 2nd element,
                // we assume it is the SD card. (This is true for M3SA and AG3.)
                var externalPaths = appContext.GetExternalFilesDirs(null);

                if (externalPaths == null || externalPaths.Length < 2)
                {
                    return null;
                }
                else
                {
                    // If there is a 2nd item, it is likely the SD card. However, if it's been
                    // ejected, the file object returned here is null since there IS a device
                    // but it is inaccessible.
                    return externalPaths[1]?.AbsolutePath;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
#nullable restore