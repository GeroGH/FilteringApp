using System;

namespace FilteringApp.UserData
{
    /// <summary>
    /// Defines how to resolve the correct file path for user data storage.
    /// Implementations can provide Tekla-based or alternative paths.
    /// </summary>
    public interface IFilePathProvider
    {
        /// <summary>
        /// Gets the absolute path to the storage file for the current user.
        /// Returns an empty string if no valid path can be determined.
        /// </summary>
        string GetFilePath(string userInitials);
    }
}