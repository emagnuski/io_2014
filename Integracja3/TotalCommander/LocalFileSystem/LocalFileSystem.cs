using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalCommander;

namespace LFS
{
    public class LocalFileSystem : IFileSystem
    {
        public void copy(string from, string to)
        {
            if (Directory.Exists(from))
            {
                this.Write(new DirectoryInfo(from), to);
            }
            else if (File.Exists(from))
            {
                this.Write(new FileInfo(from), to);
            }
        }

        public void move(string from, string to)
        {
            copy(from, to);
            remove(from);
        }

        public FileInfo getFile(string path)
        {
            return new FileInfo(path);
        }

        public List<FileInfo> getFiles(string path)
        {
            List<FileInfo> list = new List<FileInfo>();

            if (Directory.Exists(path))
            {
                DirectoryInfo dinfo = new DirectoryInfo(path);
                try
                {
                    foreach (FileInfo file in dinfo.GetFiles())
                    {
                        list.Add(file);
                    }
                }
                catch(UnauthorizedAccessException)
                {
                    return null;
                }
            }
            else if (File.Exists(path))
            {
                list.Add(new FileInfo(path));
            }
            return list;
        }

        public void remove(string path)
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



        public void rename(string path, string name)
        {
            throw new NotImplementedException();
        }
    }
}
