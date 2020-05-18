using System;
using Plugin;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;

namespace CIpher
{
    public class Zipper : IPlugin
    {
        public string name { get; } = "Заархивировать";
        public string extension = ".zip";
        public void PLoad(ref string path)
        {
            Decompress(path, path.Replace(extension, ""));
            path = path.Replace(extension, "");
        }

        public void PSave(string path)
        {
            Compress(path, path + extension);
            File.Delete(path);
        }
        public string getExtension()
        {
            return extension;
        }

        public void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }
    }
}
