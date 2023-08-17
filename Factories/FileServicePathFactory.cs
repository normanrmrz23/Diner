#nullable enable

using Diner.Services;

namespace Diner.Factories
    
{
    public static class FileServicePathFactory
    {
        private static IFileServicePaths? _fileServicePaths;

        public static void SetFileServicePaths(IFileServicePaths fileServicePaths)
        {
            _fileServicePaths = fileServicePaths;
        }

        public static IFileServicePaths GetFileServicePaths()
        {
            return _fileServicePaths ?? new FileServicePaths();
        }
    }
}
#nullable restore