namespace FilteringApp.UserData
{
    public static class UserSettingsStorage
    {
        public static void SaveTextBoxValue(string textBoxValue)
        {
            Properties.Settings.Default.UserInput = textBoxValue;
        }

        public static void SaveViewFilter(string viewFilter)
        {
            Properties.Settings.Default.UserViewFilter = viewFilter;
        }

        public static string LoadTextBoxValue()
        {
            return Properties.Settings.Default.UserInput ?? string.Empty;
        }

        public static string LoadViewFilter()
        {
            return Properties.Settings.Default.UserViewFilter ?? string.Empty;
        }
    }
}