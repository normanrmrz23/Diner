namespace Diner.Services
{
    public interface ISystemIOFile
    {
        void Copy(string sourceFileName, string destFileName);
        void Copy(string sourceFileName, string destFileName, bool overwrite);
        void Delete(string path);
        bool Exists(string path);
        FileAttributes GetAttributes(string path);
        bool IsDirectory(string path);
        void Move(string sourceFileName, string destFileName);
        Stream CreateStream(string path);
        Stream FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options);
        Stream Open(string path, FileMode mode, FileAccess access);
        Stream OpenRead(string path);
        StreamReader OpenText(string path);
        string ReadAllText(string path);
        Task<string> ReadAllTextAsync(string path);
        byte[] ReadAllBytes(string path);
        Task<byte[]> ReadAllBytesAsync(string path);
        void WriteAllText(string path, string contents);
        Task WriteAllTextAsync(string path, string contents);
        void WriteAllLines(string path, IEnumerable<string> contents);
        Task WriteAllLinesAsync(string path, IEnumerable<string> contents);

        Stream OpenWrite(string path);
        Task CopyToAsync(Stream stream, Stream destination);
    }

    public class SystemIOFile : ISystemIOFile
    {
        public Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        public Task CopyToAsync(Stream stream, Stream destination)
        {
            return stream.CopyToAsync(destination);
        }

        public void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public FileAttributes GetAttributes(string path)
        {
            return File.GetAttributes(path);
        }

        public bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return attributes.HasFlag(FileAttributes.Directory);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public Stream FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
        {
            return new FileStream(path, mode, access, share, bufferSize, options);
        }

        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            return File.Open(path, mode, access);
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public StreamReader OpenText(string path)
        {
            return File.OpenText(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public Task<byte[]> ReadAllBytesAsync(string path)
        {
            return File.ReadAllBytesAsync(path);
        }

        public Stream CreateStream(string path)
        {
            return File.Create(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public Task WriteAllTextAsync(string path, string contents)
        {
            return File.WriteAllTextAsync(path, contents);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            File.WriteAllLines(path, contents);
        }

        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents)
        {
            return File.WriteAllLinesAsync(path, contents);
        }
    }
}
