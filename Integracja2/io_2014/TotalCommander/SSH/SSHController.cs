using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander.SSH
{
    public class SSHController : IFileSystem, IDisposable
    {
        SftpClient sftp;

        public SSHController(String address, String user, String password)
        {
            sftp = new SftpClient(address, user, password);
            sftp.Connect();
        }

        public void copy(string from, string to)
        {
            using (var file = File.OpenRead(from))
            {
                sftp.UploadFile(file, to);
            }
        }

        public FileInfo getFile(string path)
        {
            return new FileInfo(path);
        }

        public List<FileInfo> getFiles(string path)
        {
            List<FileInfo> dirContent = new List<FileInfo>();
            var files = sftp.ListDirectory(path);
            foreach (var file in files)
            {
                dirContent.Add(new FileInfo(file.FullName));
            }
            return dirContent;
        }

        public void move(string from, string to)
        {
            sftp.Get(from).MoveTo(to);
        }

        public void remove(string path)
        {
            sftp.Delete(path);
        }

        public void rename(string path, string name)
        {
            sftp.RenameFile(path, name);
        }

        /// <summary>
        /// Method that enables copying files from another file systems
        /// </summary>
        public void CreateFile(String path, byte[] data)
        {
            sftp.OpenWrite(path);
            sftp.WriteAllBytes(path, data);
        }

        public void Dispose()
        {
            sftp.Disconnect();
        }
    }
}
