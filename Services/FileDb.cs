using System.Text;
using Diner.Services;
using Logger = Serilog.Log;

namespace Diner.Services
{
    public interface IFileDb
    {
        Task<IEnumerable<string>> EnumerateDirectory(string readDirectory);
        Task<IList<string>> ReadFileAsync(string filename, string readDirectory);
        Task WriteFileAsync(string filename, List<string> allLines, string writeDirectory);
        OkFailResult WriteFile(string filename, List<string> allLines, string writeDirectory);
        bool FileExists(string filename, string directory);
    }

    public class FileDb : IFileDb
    {
        private readonly ISystemIODirectory _systemIoDirectory;
        private readonly ISystemIOFile _systemIoFile;

        private static readonly object _writeFileLock = new object();
        private static readonly object _writeAsyncFileLock = new object();

        public FileDb(ISystemIODirectory systemIoDirectory, ISystemIOFile systemIoFile)
        {
            _systemIoDirectory = systemIoDirectory;
            _systemIoFile = systemIoFile;
        }

        public async Task<IEnumerable<string>> EnumerateDirectory(string ReadDirectory)
        {
            return Directory.GetFiles(ReadDirectory).Select(f => Path.GetFileName(f));
        }
        
        public async Task<IList<string>> ReadFileAsync(string filename, string readDirectory)
        {
            IList<string> allLines = new List<string>();

            if (!_systemIoDirectory.Exists(readDirectory))
                return allLines;
            
            var filePath = Path.Combine(readDirectory, filename);
            if (!_systemIoFile.Exists(filePath))
                return allLines;

            using var reader = _systemIoFile.OpenText(filePath);
            while (true)
            {
                var line = await reader.ReadLineAsync();
                if (line == null)
                    break;
                allLines.Add(line);
            }

            return allLines;
        }

        public OkFailResult WriteFile(string filename, List<string> allLines, string writeDirectory)
        {
            try
            {
                if (filename == null || writeDirectory == null || allLines == null)
                    return OkFailResult.Ok();

                if (!_systemIoDirectory.Exists(writeDirectory))
                    _systemIoDirectory.CreateDirectory(writeDirectory);
                //return OkFailResult.Fail($"{writeDirectory} does not exist");

                var filePath = Path.Combine(writeDirectory, filename);
                var tmpPath1 = Path.ChangeExtension(filePath, "tmp1");
                var tmpPath2 = Path.ChangeExtension(filePath, "tmp2");

                lock (_writeFileLock)
                {
                    var sb = new StringBuilder();
                    foreach (var line in allLines)
                    {
                        sb.AppendLine(line);
                    }

                    var lines = sb.ToString();

                    Logger.Debug($"Attempting to write {allLines.Count} lines to {filename}");
                    using (var toStream = _systemIoFile.FileStream(tmpPath1, FileMode.Create, FileAccess.Write, FileShare.Read, 5 * 1024 * 1024, FileOptions.SequentialScan))
                    {
                        using (var writer = new StreamWriter(toStream, new UTF8Encoding(false)))
                        {
                            writer.Write(lines);
                        }
                    }

                    if (!_systemIoFile.Exists(filePath))
                    {
                        _systemIoFile.Copy(tmpPath1, filePath, overwrite: true);
                        _systemIoFile.Delete(tmpPath1);
                    }
                    else
                    {
                        var acquiredLock = false;
                        var attempts = 3;
                        while (!acquiredLock && attempts > 0)
                        {
                            try
                            {
                                _systemIoFile.Copy(filePath, tmpPath2, overwrite: true);
                                _systemIoFile.Copy(tmpPath1, filePath, overwrite: true);
                                _systemIoFile.Delete(tmpPath1);
                                _systemIoFile.Delete(tmpPath2);
                                acquiredLock = true;
                            }
                            catch (IOException e)
                            {
                                if (e.Message.StartsWith("Sharing violation", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    attempts--;
                                  Logger.Error(e, $"Failed to open {filename}, attempts remaining: {attempts}");
                                    Thread.Sleep(250);
                                }
                                else
                                    throw;
                            }
                        }
                    }
                    Logger.Debug($"Write to {filename} complete");
                }

                return OkFailResult.Ok();
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failure while writing file: {filename}");
                return OkFailResult.Fail($"Failure while writing file: {filename}");
            }
        }

        public async Task WriteFileAsync(string filename, List<string> allLines, string writeDirectory)
        {
            if (filename != null && writeDirectory != null && allLines != null)
            {
                if (!_systemIoDirectory.Exists(writeDirectory))
                    return;

                var filePath = Path.Combine(writeDirectory, filename);

                try
                {
                    // File does not contain an async method to write lines to wrapping
                    // it in a Task.
                    await Task.Run(() =>
                    {
                        // To ensure that the file is not being written out by multiple threads
                        // and added a little time gap. This map possibly prevent access violoation
                        // exceptions by giving the Xamarin-Android layer time to release the file
                        // for the next write.
                        lock (_writeAsyncFileLock)
                        {
                            _systemIoFile.WriteAllLines(filePath, allLines);
                            Thread.Sleep(1000);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error saving file {filename} to {writeDirectory}.");
                    throw;
                }
            }
        }

        public bool FileExists(string filename, string directory)
        {
            if (filename == null || directory == null)
            {
                return false;
            }

            return _systemIoDirectory.Exists(directory) && _systemIoFile.Exists(Path.Combine(directory, filename));
        }
    }

}
