using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander.Controller
{
    interface IController
    {
        void orderCopy(String from, String to);
        FileInfo getFileInfo(String path);
        List<DirectoryInfo> getDirectoryInfo(string path);
        List<FileInfo> getFilesInfo(String path);
        void orderMove(String from, String to);
        void orderRemove(String path);
        void orderRename(String path, String name);
        void createArchive(List<String> fileToPack, String pathDest, String archiveType = "zip", int compressionRate = 0);
        void extractArchive(List<String> archivesList, String destinationPath);
        //saved path to each next chosen catalogue (for prev and next)
        List<FileInfo> prevCatalogue(int windowNumber);
        //returns null when there is no more prev folders
        List<FileInfo> nextCatalogue(int windowNumber);
        //use each time user opens another catalogue, needed for previous and next
        void openNext(int windowNumber, string path);
        //when opening new window to some catalogue
        void openNewWindow(int windowNumber, string cataloguePath);
    }
}
