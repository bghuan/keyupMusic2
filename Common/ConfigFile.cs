namespace keyupMusic2
{
    public partial class Common
    {
        private static string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log", "config.txt");
        private static string LogPath = Path.Combine(Directory.GetCurrentDirectory(), "log");

        public static string ConfigApiKey = "ApiKey";
        public static string ConfigMoonTimeLocation = "MoonTimeLocation";
        public static string ConfigMoonTimeSize = "MoonTimeSize";
        public static string ConfigCurrentId = "CurrentId";
        public static string ConfigCurrentFile = "CurrentFile";
        public static string ConfigFormShow = "FormShow";
        public static string ConfigLocation = "Location";

        public static string ConfigValue(string key, string value = "ddddddd")
        {
            if (!File.Exists(LogPath)) { Directory.CreateDirectory(LogPath); }
            if (!File.Exists(ConfigFilePath)) { File.Create(ConfigFilePath).Close(); }
            List<string> lines = File.ReadAllText(ConfigFilePath).Split("\n").ToList();

            if (value == "ddddddd")
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith(key + "="))
                        return lines[i].Substring(key.Length + 1).Trim();
                    if (lines[i].StartsWith(key + " ="))
                        return lines[i].Substring(key.Length + 2).Trim();
                }
                return "";
            }

            bool keyFound = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(key + "=") || lines[i].StartsWith(key + " ="))
                {
                    lines[i] = $"{key} = {value}";
                    keyFound = true;
                    break;
                }
            }

            if (!keyFound)
                lines.Add($"{key} = {value}");

            File.WriteAllText(ConfigFilePath, string.Join("\n", lines));
            return "";
        }

        public static string GetConfigValue(string key)
        {
            if (!File.Exists(ConfigFilePath)) { File.Create(ConfigFilePath).Close(); return ""; }
            var all = File.ReadAllText(ConfigFilePath);
            List<string> lines = all.Split("\n").ToList();
            for (int i = 0; i < lines.Count; i++)
                if (lines[i].StartsWith(key + "="))
                    return lines[i].Substring(key.Length + 1).Trim();
            return "";
        }
        public static void SetConfigValue(string key, string value)
        {
            if (!File.Exists(ConfigFilePath)) { File.Create(ConfigFilePath).Close(); }
            List<string> lines = File.ReadAllText(ConfigFilePath).Split("\n").ToList();
            bool keyFound = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(key + "="))
                {
                    lines[i] = $"{key}={value}";
                    keyFound = true;
                    break;
                }
            }

            if (!keyFound)
                lines.Add($"{key}={value}");

            File.WriteAllText(ConfigFilePath, string.Join("\n", lines));
        }
    }
}
