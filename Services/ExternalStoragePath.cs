using Diner.Abstractions;

namespace Diner.Services
{
    internal partial class ExternalStoragePath : IExternalStoragePath
    {
#if NET && !(WINDOWS || IOS || ANDROID)
        string IExternalStoragePath.LocalApplicationData => throw new NotImplementedException();

        string IExternalStoragePath.ExternalStorageRoot => throw new NotImplementedException();

        string IExternalStoragePath.FilesFolder => throw new NotImplementedException();

        string IExternalStoragePath.ApplicationData => throw new NotImplementedException();

        string IExternalStoragePath.BackupPathBase => throw new NotImplementedException();
#endif
    }
}