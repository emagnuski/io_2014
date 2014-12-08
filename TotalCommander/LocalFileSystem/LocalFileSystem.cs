using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFS
{
    //TODO: SPRAWDZANIE ŚCIEŻKI
    public class LocalFileSystem : IFileSystem
    {
        public void Copy(string from, string to, IFileSystem fileSystem)
        {
            if (Directory.Exists(from))
            {
                fileSystem.Write(new DirectoryInfo(from), to);
            }
            else if (File.Exists(from))
            {
                fileSystem.Write(new FileInfo(from), to);
            }
        }

        public void Move(string from, string to, IFileSystem fileSystem)
        {
            Copy(from, to, fileSystem);
            Remove(from);
        }

        public FileInfo GetFile(string path)
        {
            return new FileInfo(path);
        }

        public List<FileInfo> GetFiles(string path)
        {
            List<FileInfo> list = new List<FileInfo>();

            if (Directory.Exists(path))
            {
                DirectoryInfo dinfo = new DirectoryInfo(path);
                foreach (FileInfo file in dinfo.GetFiles())
                {
                    list.Add(file);
                }
            }
            else if (File.Exists(path))
            {
                list.Add(new FileInfo(path));
            }
            return list;
        }

        public void Remove(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else
            {
                File.Delete(path);
            }
        }

        public void Write(FileInfo file, string path)
        {
            file.CopyTo(path);
        }

        public void Write(DirectoryInfo dir, string path)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                Write(file, Path.Combine(path, file.Name));
            }
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                string subdirPath = Path.Combine(path, subdir.Name);
                if (!Directory.Exists(subdirPath))
                {
                    Directory.CreateDirectory(subdirPath);
                }
                Write(subdir, subdirPath);
            }
        }
    }
}
