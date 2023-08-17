using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Diner.Services;

namespace Diner.Services
{
    /// <summary>
    /// Provides generic access to the file system.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Represents the base folder for "public" files the user is intended
        /// to be able to manipulate.
        /// </summary>
        string PublicStoragePath { get; }

        /// <summary>
        /// Represents the base folder for "private" files the user is not
        /// intended to be able to manipulate.
        /// </summary>
        string PrivateStoragePath { get; }

        /// <summary>
        /// Creates a directory at the given path.
        /// </summary>        
        void CreateDirectory(string path);

        /// <summary>
        /// Deletes a directory at the given path.
        /// </summary>
        void DeleteDirectory(string path);

        /// <summary>
        /// Returns true if a directory exists at the indicated path.
        /// </summary>
        bool DirectoryExists(string directoryPath);

        /// <summary>
        /// Returns an enumeration of paths to the items contined in the directory
        /// at the indicated path.
        /// </summary>
        IEnumerable<string> EnumerateDirectory(string directoryPath);

        /// <summary>
        /// Returns true if the given path represents a directory.
        /// </summary>
        bool IsDirectory(string path);

        /// <summary>
        /// Deletes the file at the given path. Does not throw if the file already
        /// does not exist.
        /// </summary>
        void DeleteFile(string fileWithPath);

        /// <summary>
        /// Returns true if a file exists at the indicated path.
        /// </summary>
        bool FileExists(string filePath);

        /// <summary>
        /// Open a file at an arbitrary path for reading, or throw if it doesn't exist.
        /// </summary>
        Stream OpenRead(string path);

        /// <summary>
        /// Open a file at an arbitrary path for writing. 
        /// </summary>
        /// <remarks>
        /// If the file exists, its contents will be replaced.
        /// </remarks>
        Stream OpenWrite(string path);

        // I'm agitated by ReadAllBytesAsync(), it's much easier to mock if there's
        // only one way to read a file. Unfortunately, reading all bytes from a
        // stream is laborious, so it needs to be around. I could eliminate it with
        // a helper class somewhere else, but it's worrying too much.

        /// <summary>
        /// Returns all bytes in a file at the specified path.
        /// </summary>
        Task<byte[]> ReadAllBytesAsync(string path);

        /// <summary>
        /// Combines a file name with a folder path.
        /// </summary>
        string CombinePaths(string folder, string fileName);

        /// <summary>
        /// Deletes a directory and, if it is not empty, all of its contents.
        /// </summary>
        void DeleteDirectoryRecursive(string path);

        /// <summary>
        /// Copies a file. 
        /// </summary>
        void CopyFile(string sourcePath, string destinationPath);
    }

    public class FileService : IFileService
    {
        private readonly IFileServicePaths _fileServicePaths;
        private readonly ISystemIODirectory _systemIoDirectory;
        private readonly ISystemIOFile _systemIoFile;

        // #TechDebt
        // IFileServicePaths can and should be replaced with IExternalStorage
        public string PublicStoragePath => _fileServicePaths.PublicStoragePath;
        public string PrivateStoragePath => _fileServicePaths.PrivateStoragePath;

        public FileService(
            IFileServicePaths fileServicePaths,
            ISystemIODirectory systemIoDirectory,
            ISystemIOFile systemIoFile)
        {
            _fileServicePaths = fileServicePaths;
            _systemIoDirectory = systemIoDirectory;
            _systemIoFile = systemIoFile;
        }

        public void CreateDirectory(string path)
        {
            _systemIoDirectory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            _systemIoDirectory.Delete(path);
        }

        public void DeleteDirectoryRecursive(string path)
        {
            _systemIoDirectory.Delete(path, true);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return _systemIoDirectory.Exists(directoryPath);
        }

        public IEnumerable<string> EnumerateDirectory(string directoryPath)
        {
            return _systemIoDirectory.EnumerateFileSystemEntries(directoryPath);
        }

        public bool IsDirectory(string path)
        {
            var attributes = _systemIoFile.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Directory);
        }

        public void DeleteFile(string fileWithPath)
        {
            if (_systemIoFile.Exists(fileWithPath))
            {
                _systemIoFile.Delete(fileWithPath);
            }
        }

        public bool FileExists(string filePath)
        {
            return _systemIoFile.Exists(filePath);
        }

        public Stream OpenRead(string path)
        {
            return _systemIoFile.OpenRead(path);
        }

        public Stream OpenWrite(string path)
        {
            return _systemIoFile.OpenWrite(path);
        }

        public string CombinePaths(string folderPath, string fileName)
        {
            return Path.Combine(folderPath, fileName);
        }

        public Task<byte[]> ReadAllBytesAsync(string path)
        {
            return _systemIoFile.ReadAllBytesAsync(path);
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            _systemIoFile.Copy(sourcePath, destinationPath);
        }
    }
}
