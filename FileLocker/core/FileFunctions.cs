using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using NETCore.Encrypt;

namespace FileLocker.core
{
    public class FileFunctions
    {
        public static string OpenFile(byte[] bytes, string fileName)
        {
            var appPath = Directory.GetCurrentDirectory();
            var showPath = $"{appPath}/fileShowing";
            var showPathExist = Directory.Exists(showPath);
            if (!showPathExist) Directory.CreateDirectory(showPath);
            var filePath = $"{showPath}/{fileName}";
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes);
                fs.Close();
            }
            return filePath;
        }

        public static void ClearFileBufferDir()
        {
            try
            {
                var appPath = Directory.GetCurrentDirectory();
                var showPath = $"{appPath}/fileShowing";
                if (Directory.Exists(showPath))
                {
                    Directory.Delete(showPath, true);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"FileFunctions ClearFileBufferDir Error: {e.Message}");
            }
        }

        public static byte[] Encode(byte[] bytes, string password)
        {
            if (bytes.Length == 0)
            {
                MessageBox.Show("File size is zero");
                return null;
            }
            var sha = SHA256.Create();
            var sb = new StringBuilder();
            var keyBytes =  sha.ComputeHash(Encoding.UTF8.GetBytes(password.ToCharArray())).ToList();
            keyBytes.ForEach(x => sb.Append(x));
            var key = new string(sb.ToString().Take(32).ToArray());
            var iv = new string(sb.ToString().Take(16).ToArray());
            var encryptedBytes = EncryptProvider.AESEncrypt(bytes, key, iv);
            return encryptedBytes;
        }

        public static byte[] Decode(byte[] bytes, string password)
        {
            var sha = SHA256.Create();
            var sb = new StringBuilder();
            var keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password.ToCharArray())).ToList();
            keyBytes.ForEach(x => sb.Append(x));
            var key = new string(sb.ToString().Take(32).ToArray());
            var iv = new string(sb.ToString().Take(16).ToArray());
            var encryptedBytes = EncryptProvider.AESDecrypt(bytes, key, iv);
            return encryptedBytes;
        }
    }
}
