using LFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalCommander._7Zip;
using TotalCommander.SSH;

namespace TotalCommander.Controller
{
    public class Controller : IController
    {
        private IArchive7Zip archivizer { get; set; }
        private IFileSystem FileSystem { get; set; }
        private IFileSystem SSH { get; set; }
        private IFileSystem Dropbox { get; set; }
        //int - window number, strings - catalogue paths
        private Dictionary<int, List<string>> prevCatalogues { get; set; }
        //position in catalogue list prev and next catalogue
        private Dictionary<int,int> cataloguePos{get; set;}
        private List<string> undoList;
        private int catPos;

        public Controller()
        {
            this.archivizer = new Archive7Zip();
            this.prevCatalogues = new Dictionary<int, List<string>>();
            this.cataloguePos = new Dictionary<int, int>();
            this.undoList = new List<string>();
            this.FileSystem = new LocalFileSystem();
            this.Dropbox = new DropBox.DropBox();
        }

        //returns null when there is no more prev folders
        public List<FileInfo> prevCatalogue(int windowNumber)
        {
            if (cataloguePos.ElementAt(windowNumber).Equals(0))
                return null;
            else
            {
                cataloguePos[windowNumber]--;
                return getCatalogue(windowNumber);
            }
        }

        //returns null when there is no more prev folders
        public List<FileInfo> nextCatalogue(int windowNumber)
        {
            cataloguePos[windowNumber]++;
            return getCatalogue(windowNumber);
        }

        private List<FileInfo> getCatalogue(int windowNumber)
        {
            if (prevCatalogues.TryGetValue(windowNumber, out undoList))
            {
                
                if (cataloguePos.TryGetValue(windowNumber, out catPos))
                {
                    if (catPos > undoList.Count) return null;
                    else
                    return getFilesInfo(undoList.ElementAt(catPos));
                }
                else return null;
            }
            else return null;
        }

        //to do: delete any list after this position
        /**************************************************************************************************************************/
        public void openNext(int windowNumber, string path)
        {
            prevCatalogues[windowNumber].Add(path);
            cataloguePos[windowNumber]++;
        }

        //when opening new window to some catalogue
        public void openNewWindow(int windowNumber, string cataloguePath)
        {
            List<string> list = new List<string>();
            list.Add(cataloguePath);
            this.prevCatalogues.Add(windowNumber, list);
            this.cataloguePos.Add(windowNumber, 0);
            
        }

        public void createArchive(List<String> fileToPack, String pathDest, String archiveType = "zip", int compressionRate = 0)
        {
            this.archivizer.createArchive(fileToPack,pathDest,archiveType,compressionRate);
        }

        public void extractArchive(List<String> archivesList, String destinationPath)
        {
            foreach(String archName in archivesList)
            this.archivizer.extractArchive(archName, destinationPath);
        }

        //ask about this method, what about copying between ssh and drop for example
        public void orderCopy(string from, string to)
        {
            throw new NotImplementedException();
        }

        public FileInfo getFileInfo(string path)
        {
            if (path.StartsWith("dropbox.com"))
                return Dropbox.getFile(path);
            else if (path.StartsWith("\\"))
                return SSH.getFile(path);
            else return FileSystem.getFile(path);

        }

        public List<FileInfo> getFilesInfo(string path)
        {
            if (path.StartsWith("www.dropbox.com"))
                return Dropbox.getFiles(path);
            else if (path.StartsWith("/"))
                return SSH.getFiles(path);
            else
            {
                return FileSystem.getFiles(path);
            }
        }

        public List<DirectoryInfo> getDirectoryInfo(string path)
        {
            List<DirectoryInfo> dInList = new List<DirectoryInfo>();
            if (Directory.Exists(path))
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                foreach (DirectoryInfo di in dInfo.GetDirectories())
                {
                    dInList.Add(di);
                }
            }
            return dInList;
        }

        //the same as copy
        public void orderMove(string from, string to)
        {
            throw new NotImplementedException();
        }



        public void orderRemove(string path)
        {
            if (path.StartsWith("www.dropbox.com"))
                Dropbox.remove(path);
            else if (path.StartsWith("/"))
                SSH.remove(path);
            else FileSystem.remove(path);
        }

        public void orderRename(string path, string name)
        {
            if (path.StartsWith("www.dropbox.com"))
                Dropbox.rename(path, name);
            else if (path.StartsWith("/"))
                SSH.rename(path, name);
            else FileSystem.rename(path, name);
        }
        
    }
}
