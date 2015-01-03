using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander.DropBox
{
    interface IFileSystem
    {
        void CopyFile(String from, String to);
        byte[] GetFile(String path);
        void MoveFile(String from, String to);
        void DeleteFile(String path);
        void RenameFile(String path, String name);
        void UploadFile(String path, String fileName, byte[] fileData);
        void CreateFolder(String name);

    }
}
