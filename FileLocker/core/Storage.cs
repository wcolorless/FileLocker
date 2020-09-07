using System;
using System.Collections.Generic;
using System.Text;

namespace FileLocker.core
{
    [Serializable]
    public class Storage
    {
        public string Name { get; set; }
        public bool IsPasswordSet { get; set; }
        public List<FileItem> Files { get; set; }
    }
}
