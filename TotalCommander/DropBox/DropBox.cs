using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DropNet.Models;
using DropNet;
using System.IO;
using System.Threading.Tasks;

namespace TotalCommander.DropBox
{
    class DropBox : IFileSystem
    {
        DropNetClient client;

        public DropBox()
        {
        }

        public void connect()
        {
            //trzeba bedzie wygenerowac dla wlasciwej aplikacji
            client = new DropNetClient("l1eott9plu8q24s", "o5z985cqb6avzfp", "asl6k3cej6rbyeuy", "56j4g0z8chb5rvb");
        }

        public void CopyFile(string from, string to)
        {
            client.Copy(from, to);
        }

        public byte[] GetFile(string path)
        {
            return client.GetFile(path);
        }

        public void MoveFile(string from, string to)
        {
            client.Move(from, to);
        }

        public void DeleteFile(string path)
        {
            client.Delete(path);
        }

        public void RenameFile(string path, string name)
        {
            throw new NotImplementedException();
        }

        public void UploadFile(string path, string fileName, byte[] fileData)
        {
            client.UploadFile(path, fileName, fileData);
        }

        public void CreateFolder(string path)
        {
            client.CreateFolder(path);
        }

    }
}
