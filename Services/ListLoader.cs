using System;
using System.Globalization;
using Diner.Abstractions;
using Yelp.Api.Models;

namespace Diner.Services
{
    public interface IListLoader
    {
        Task<List<string>> LoadAsync(string filename);
        Task<IEnumerable<string>> LoadAllListsAsync();

    }

    public class ListLoader : IListLoader
    {
        protected readonly CultureInfo _provider = CultureInfo.InvariantCulture;
        private readonly IFileDb _fileDb;
        private readonly string _listsDirectoryPath;

        public ListLoader(
            IFileDb fileDb,
            IExternalStoragePath externalStoragePath
        )
        {
            _fileDb = fileDb;
            var appDataPath = externalStoragePath.ApplicationData;
            _listsDirectoryPath = Path.Combine(appDataPath, "CustomLists");
        }

        public async Task<List<string>> LoadAsync(string filename)
        {
            List<string> businesses = new();
            if (_fileDb.FileExists(filename, _listsDirectoryPath))
            {
                var allLines = await _fileDb.ReadFileAsync(filename, _listsDirectoryPath);
                for (int i = 1; i < allLines.Count - 1;i++)
                {
                    businesses.Add(allLines[i]);
                }

            }

            return businesses;
        }

        public async Task<IEnumerable<string>> LoadAllListsAsync()
        {
            var allLists = await _fileDb.EnumerateDirectory(_listsDirectoryPath);
            return allLists;
        }

    }
}
