using System;
namespace Diner.Services
{
    /// <summary>
    /// Represents platform-specific base file paths used by DVM API.
    /// </summary>
    public interface IFileServicePaths
    {
        /// <summary>
        /// "Public" storage is for user-facing files like log captures,
        /// survey files, etc.
        /// </summary>
        /// <remarks>
        /// This path should be a root and exclude any AI-specific directories.
        /// It will likely be external storage on Android.
        /// </remarks>
        public string PublicStoragePath { get; }

        /// <summary>
        /// "Private" storage is meant for the DVM Settings and other internal
        /// files the user should not access.
        /// </summary>
        public string PrivateStoragePath { get; }
    }

    public partial class FileServicePaths : IFileServicePaths
    {
#if NET && !(WINDOWS || IOS || ANDROID)
        string IFileServicePaths.PublicStoragePath => throw new NotImplementedException();

        string IFileServicePaths.PrivateStoragePath => throw new NotImplementedException();
#endif
    }
}