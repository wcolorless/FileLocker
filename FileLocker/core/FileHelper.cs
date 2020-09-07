using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FileLocker.core
{
    public class FileHelper
    {
        public static void OpenFileInFolder(string path)
        {
            try
            {
                var process = new Process {StartInfo = new ProcessStartInfo(path) {UseShellExecute = true}};
                process.Start();
            }
            catch (Exception e)
            {
                throw new Exception($"FileHelper OpenFileInFolder Error: {e.Message}");
            }
        }

        public static string SaveFileToDir(byte[] bytes, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes);
                fs.Close();
            }
            return path;
        }
    }


}
