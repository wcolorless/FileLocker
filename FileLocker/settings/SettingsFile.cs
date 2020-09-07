using System;
using System.Collections.Generic;
using System.Text;

namespace FileLocker.settings
{
    [Serializable]
    public class SettingsFile
    {
        public List<FileStorage> FileStorages { get; set; }
    }
}
