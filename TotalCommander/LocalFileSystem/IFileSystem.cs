using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFS
{
    public interface IFileSystem
    {
        void Copy(String from, String to, IFileSystem fileSystem);
        void Move(String from, String to, IFileSystem fileSystem);
        FileInfo GetFile(String path);
        List<FileInfo> GetFiles(String path);
        void Remove(String path);
        void Write(FileInfo file, string path);
        void Write(DirectoryInfo dir, string path);
    }
}
