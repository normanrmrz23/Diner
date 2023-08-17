using System.Collections.Generic;
using System.IO;

namespace Diner.Services
{
    public interface ISystemIODirectory
    {
        void CreateDirectory(string path);
        void Delete(string path, bool recurse = false);
        IEnumerable<string> EnumerateFiles(string path);
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);
        IEnumerable<string> EnumerateDirectories(string path);
        IEnumerable<string> EnumerateFileSystemEntries(string path);
        bool Exists(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        string[] GetDirectories(string path);
    }

    public class SystemIODirectory : ISystemIODirectory
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void Delete(string path, bool recurse = false)
        {
            Directory.Delete(path, recurse);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
    }
}