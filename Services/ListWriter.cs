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
                    //Logger.Debug("=== GenerateAndSaveFile Processing {IsNot} On MainThread ===", _mainThread.IsMainThread ? "Is" : "Not");

                    var infoFileLines = new List<string> { "Start List" };
                    AddInfoLines(business, infoFileLines);


                    // Count is 1 less than the count because we have 1 placeholder.
                   // ciSurveyFileLines.Add(string.Format(_provider, "{0} = {1}:", SurveyOptionsFileStrings.Records, ciReadings.Length - 1));


                    //AddReadingLines(ciReadings, ciSurveyFileLines);
                    infoFileLines.Add("End");
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

        protected virtual void AddInfoLines(BusinessResponse businessResponse, List<string> infoLines)
		{
            var infoLineFormatString = "{0} = {1}";
			infoLines.Add(string.Format(_provider, infoLineFormatString, "Name", businessResponse.Name));
        }


    }
}

