using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using FileLocker.core;

namespace FileLocker.setup
{
    public class StorageLoader
    {
        public static void Save(Storage storage, string path)
        {
            using (var fs  = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, storage);
                fs.Close();
            }
        }

        public static Storage Load(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                var result = bf.Deserialize(fs) as Storage;
                fs.Close();
                return result;
            }
        }
    }
}
