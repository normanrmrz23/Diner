#nullable enable
using System;
using System.IO;

namespace Diner.Services
{
    internal partial class ExternalStoragePath
    {
        public ExternalStoragePath()
        {
            DefineFolders();
        }

        // [MOB-979] on iOS LocalApplicationData is /Library/Files  (SQLite DB location)
        public string LocalApplicationData { get; private set; }

        public string ExternalStorageRoot => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public string FilesFolder => "Files";

        // [MOB-979] on iOS ApplicationData is /Documents/Files   (public folder structure e.g. PS, CI, DVM, Pictures, etc.)
        public string ApplicationData { get; private set; }

        public string? BackupPathBase { get; private set; }

        private void DefineFolders()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = Path.Combine(documents, "..", "Library");

            // this Files subfolder is not the same as the public FilesFolder property
            // they just happen to be named the same, so let's not confuse them
            const string libraryFilesSubfolder = "Files";
            var directoryName = Path.Combine(library, libraryFilesSubfolder);

            var directoryInfo = Directory.Exists(directoryName)
                ? new DirectoryInfo(directoryName)
                : Directory.CreateDirectory(directoryName);

            ApplicationData = Path.Combine(documents, FilesFolder);
            BackupPathBase = documents;
            LocalApplicationData = directoryInfo.FullName;
        }
    }
}
#nullable restore