using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileLocker.core
{
    [Serializable]
    public class FileItem
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public List<FileBlock> FileBlocks { get; set; }

        public static byte[] GetBytes(FileItem fileItem)
        {
            var bytes = new List<byte>();
            foreach (var block in fileItem.FileBlocks)
            {
                bytes.AddRange(block.Array);
            }

            return bytes.ToArray();
        }
    }

    [Serializable]
    public class FileBlock
    {
        public byte[] Array { get; set; }
    }
}
