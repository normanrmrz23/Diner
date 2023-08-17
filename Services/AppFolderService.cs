using System;
using Diner.Abstractions;
using Logger = Serilog.Log;

namespace Diner.Services
{
    public interface IAppFolderService
    {
        void EnsureStructure();
        bool EnsureDirectory(string directory);
    }

    public class AppFolderService : IAppFolderService
    {
        private readonly IExternalStoragePath _externalStoragePath;
        private readonly ISystemIODirectory _systemIoDirectory;

        public AppFolderService(
            IExternalStoragePath externalStoragePath,
            ISystemIODirectory systemIoDirectory)
        {
            _externalStoragePath = externalStoragePath;
            _systemIoDirectory = systemIoDirectory;
        }

        public void EnsureStructure()
        {
            var appDataPath = _externalStoragePath.ApplicationData;

            EnsureDirectory(Path.Combine(appDataPath, AppConstants.AppDataFilesCustomListsPath));
        }

        public bool EnsureDirectory(string directory)
        {
            try
            {
                if (!_systemIoDirectory.Exists(directory))
                {
                    _systemIoDirectory.CreateDirectory(directory);
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to ensure directory {directory}: {e}");
                return false;
            }
        }
    }
}
