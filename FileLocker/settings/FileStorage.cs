using System;
using System.Collections.Generic;
using System.Text;

namespace FileLocker.settings
{
    [Serializable]
    public class FileStorage
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
