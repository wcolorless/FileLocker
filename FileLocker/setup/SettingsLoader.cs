using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using FileLocker.settings;

namespace FileLocker.setup
{
    public class SettingsLoader
    {
        private static readonly string path = $"{Directory.GetCurrentDirectory() }/settings.fls";

        public static SettingsFile GetSettings()
        {
            if (!File.Exists(path)) return CreateSettings();
            return LoadSettings();
        }

        public static void SaveSettings(SettingsFile settingsFile)
        {
            WriteSettings(settingsFile);
        }

        private static SettingsFile CreateSettings()
        {
            var settings = new SettingsFile();
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bf = new BinaryFormatter();
            bf.Serialize(stream, settings);
            stream.Close();
            return settings;
        }

        private static SettingsFile LoadSettings()
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var bf = new BinaryFormatter();
            var result = bf.Deserialize(stream) as SettingsFile;
            stream.Close();
            return result;
        }

        private static void WriteSettings(SettingsFile settingsFile)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var bf = new BinaryFormatter();
            bf.Serialize(stream, settingsFile);
            stream.Close();
        }
    }
}
