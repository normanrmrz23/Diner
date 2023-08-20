using System;
using System.Globalization;
using Diner.Abstractions;
using Logger= Serilog.Log;
using Yelp.Api.Models;

namespace Diner.Services
{
    public interface IListWriter
    {
        Task<OkFailResult> WriteAsync(string filename, BusinessResponse business);
        OkFailResult WriteExisting(string filename, BusinessResponse business);
        OkFailResult DeleteExisting(string filename);
    }
    public class ListWriter : IListWriter
	{
        protected readonly CultureInfo _provider = CultureInfo.InvariantCulture;
        private readonly IFileDb _fileDb;
        private readonly Serilog.ILogger _logger;
        private readonly string _listsDirectoryPath;

        public ListWriter(
            IFileDb fileDb,
            IExternalStoragePath externalStoragePath
            )
		{
			_fileDb = fileDb;
            var appDataPath = externalStoragePath.ApplicationData;
            _listsDirectoryPath = Path.Combine(appDataPath, "CustomLists");
        }

        public async Task<OkFailResult> WriteAsync(string filename, BusinessResponse business)
        {
            var retCode = OkFailResult.Ok();

            await Task.Run(() =>
            {
                try
                {
                    var infoFileLines = new List<string>();
                    AddInfoLines(business, infoFileLines);

                    var path = FileSystem.Current.AppDataDirectory;
                    var fullPath = Path.Combine(path, filename);
                    File.WriteAllLines(fullPath, infoFileLines);

                    _fileDb.WriteFile(filename, infoFileLines, _listsDirectoryPath);
                }
                catch (Exception e)
                {
                    retCode = OkFailResult.Fail(e.Message);
                }

                return Task.CompletedTask;
            });

            return retCode;
        }

        public OkFailResult WriteExisting(string filename, BusinessResponse business)
        {
            var retCode = OkFailResult.Ok();
            try
            {
                var infoFileLines = new List<string>();
                AddInfoLines(business, infoFileLines);
                _fileDb.AppendToExistingFile(filename, infoFileLines, _listsDirectoryPath);
            }
            catch(Exception e)
            {
                retCode = OkFailResult.Fail(e.Message);
            }
            return retCode;
        }

        public OkFailResult DeleteExisting(string filename)
        {
            _fileDb.DeleteFile(filename, _listsDirectoryPath);
            return OkFailResult.Ok();
        }
        protected virtual void AddInfoLines(BusinessResponse businessResponse, List<string> infoLines)
		{
            var infoLineFormatString = "{0}";
			infoLines.Add(string.Format(_provider, infoLineFormatString, businessResponse.Id));
        }


    }
}

