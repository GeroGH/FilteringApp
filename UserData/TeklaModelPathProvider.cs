using System;
using System.IO;
using Tekla.Structures.Model;

namespace FilteringApp.UserData
{
    /// <summary>
    /// Resolves a valid file path for user data based on the current Tekla model.
    /// The file is stored in the model's "attributes" folder.
    /// </summary>
    public class TeklaModelPathProvider : IFilePathProvider
    {
        public string GetFilePath(string userInitials)
        {
            try
            {
                var model = new Model();
                if (model == null || !model.GetConnectionStatus())
                    return string.Empty;

                var info = model.GetInfo();
                if (info == null || string.IsNullOrWhiteSpace(info.ModelPath))
                    return string.Empty;

                var attrFolder = Path.Combine(info.ModelPath, "attributes");
                if (!Directory.Exists(attrFolder))
                    Directory.CreateDirectory(attrFolder);

                var fileName = $"FilteringAppSettings_{userInitials}.json";
                return Path.Combine(attrFolder, fileName);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
