using System;
namespace Diner.Abstractions
{
    public interface IExternalStoragePath
    {
        string LocalApplicationData { get; }

        string ExternalStorageRoot { get; }

        string FilesFolder { get; }

        string ApplicationData { get; }

        string? BackupPathBase { get; }
    }
}

