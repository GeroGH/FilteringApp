using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Handles safe JSON-based saving and loading of FilteringApp user settings.
    /// Stores the TextBox input and current view filter in the model's attributes folder.
    /// </summary>
    public class UserSettingsStorage
    {
        private readonly string filePath;

        /// <summary>
        /// Small DTO that represents the stored data.
        /// </summary>
        private class SettingsData
        {
            public string TextBoxUserInput { get; set; } = string.Empty;
            public string CurrentViewFilter { get; set; } = string.Empty;
            public DateTime LastSaved { get; set; } = DateTime.Now;
        }

        public UserSettingsStorage()
        {
            try
            {
                var model = new Model();

                if (model == null || !model.GetConnectionStatus())
                {
                    this.filePath = string.Empty;
                    return;
                }

                var info = model.GetInfo();

                if (info == null || string.IsNullOrWhiteSpace(info.ModelPath))
                {
                    this.filePath = string.Empty;
                    return;
                }

                var attrFolder = Path.Combine(info.ModelPath, "attributes");

                if (!Directory.Exists(attrFolder))
                {
                    try { Directory.CreateDirectory(attrFolder); }
                    catch { /* ignore */ }
                }

                this.filePath = Path.Combine(attrFolder, "FilteringAppSettings.json");
            }
            catch
            {
                this.filePath = string.Empty;
            }
        }

        public bool IsReady => !string.IsNullOrEmpty(this.filePath);

        private SettingsData LoadExistingData()
        {
            if (!this.IsReady || !File.Exists(this.filePath))
                return new SettingsData();

            try
            {
                using (var fs = new FileStream(this.filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    var data = JsonSerializer.Deserialize<SettingsData>(json);
                    return data ?? new SettingsData();
                }
            }
            catch
            {
                return new SettingsData();
            }
        }

        private void SaveData(SettingsData data)
        {
            if (!this.IsReady)
                return;

            try
            {
                data.LastSaved = DateTime.Now;

                var json = JsonSerializer.Serialize(
                    data,
                    new JsonSerializerOptions { WriteIndented = true });

                using (var fs = new FileStream(
                    this.filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.Read,
                    4096,
                    FileOptions.WriteThrough))
                using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    writer.Write(json);
                }
            }
            catch
            {
                // ignore save errors silently
            }
        }

        /// <summary>
        /// Saves both the textbox input and view filter together.
        /// </summary>
        public void SaveSettings(string textBoxValue, string currentViewFilter)
        {
            var data = new SettingsData
            {
                TextBoxUserInput = textBoxValue ?? string.Empty,
                CurrentViewFilter = currentViewFilter ?? string.Empty
            };

            this.SaveData(data);
        }

        /// <summary>
        /// Saves only the textbox input, keeping the existing view filter.
        /// </summary>
        public void SaveTextBoxValue(string textBoxValue)
        {
            var data = this.LoadExistingData();
            data.TextBoxUserInput = textBoxValue ?? string.Empty;
            this.SaveData(data);
        }

        /// <summary>
        /// Saves only the current view filter, keeping the existing textbox value.
        /// </summary>
        public void SaveViewFilter(string viewFilter)
        {
            var data = this.LoadExistingData();
            data.CurrentViewFilter = viewFilter ?? string.Empty;
            this.SaveData(data);
        }

        /// <summary>
        /// Loads the previously stored textbox input and view filter from the JSON file.
        /// Returns empty strings if file not found or invalid.
        /// </summary>
        public void LoadSettings(out string textBoxValue, out string viewFilter)
        {
            textBoxValue = string.Empty;
            viewFilter = string.Empty;

            if (!this.IsReady || !File.Exists(this.filePath))
                return;

            try
            {
                using (var fs = new FileStream(
                    this.filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    var data = JsonSerializer.Deserialize<SettingsData>(json);

                    if (data != null)
                    {
                        textBoxValue = data.TextBoxUserInput ?? string.Empty;
                        viewFilter = data.CurrentViewFilter ?? string.Empty;
                    }
                }
            }
            catch
            {
                // ignore load errors silently
            }
        }
    }
}
