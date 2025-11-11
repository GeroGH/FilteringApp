using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FilteringApp.UserData
{
    /// <summary>
    /// Provides static methods for safely saving and loading
    /// user-specific text box input and view filter data.
    /// </summary>
    public static class UserSettingsStorage
    {
        private static readonly object LockObject = new object();
        private static string filePath = string.Empty;

        /// <summary>
        /// Initializes the storage location based on the given Tekla path provider and user initials.
        /// Must be called once during application startup.
        /// </summary>
        public static void Initialize(IFilePathProvider pathProvider, string userInitials)
        {
            if (pathProvider == null)
                throw new ArgumentNullException(nameof(pathProvider));

            filePath = pathProvider.GetFilePath(userInitials);
        }

        private static bool IsReady => !string.IsNullOrWhiteSpace(filePath);

        public static void SaveTextBoxValue(string textBoxValue)
        {
            if (!IsReady)
                return;

            lock (LockObject)
            {
                var data = LoadInternal();
                data.TextBoxUserInput = textBoxValue ?? string.Empty;
                SaveInternal(data);
            }
        }

        public static void SaveViewFilter(string viewFilter)
        {
            if (!IsReady || string.IsNullOrWhiteSpace(viewFilter))
                return;

            lock (LockObject)
            {
                var data = LoadInternal();
                data.CurrentViewFilter = viewFilter ?? string.Empty;
                SaveInternal(data);
            }
        }

        public static string LoadTextBoxValue()
        {
            if (!IsReady)
                return string.Empty;

            lock (LockObject)
            {
                var data = LoadInternal();
                return data.TextBoxUserInput ?? string.Empty;
            }
        }

        public static string LoadViewFilter()
        {
            if (!IsReady)
                return string.Empty;

            lock (LockObject)
            {
                var data = LoadInternal();
                return data.CurrentViewFilter ?? string.Empty;
            }
        }

        private static SettingsData LoadInternal()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new SettingsData();

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var data = JsonSerializer.Deserialize<SettingsData>(json);
                return data ?? new SettingsData();
            }
            catch
            {
                return new SettingsData();
            }
        }

        private static void SaveInternal(SettingsData data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch
            {
                // Silent fail — safe fallback
            }
        }

        /// <summary>
        /// Internal DTO for serialization of settings.
        /// </summary>
        private class SettingsData
        {
            public string TextBoxUserInput { get; set; } = string.Empty;
            public string CurrentViewFilter { get; set; } = string.Empty;
        }
    }
}