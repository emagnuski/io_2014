using LFS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalCommander._7Zip;
using TotalCommander.Journals;
using TotalCommander.SSH;

namespace TotalCommander.Controller
{
    public class Controller : IController
    {
        private IArchive7Zip archivizer { get; set; }
        private IFileSystem FileSystem { get; set; }
        private IFileSystem SSH { get; set; }
        private IFileSystem Dropbox { get; set; }
        private IJournalManager Journals { get; set; }
        //int - window number, strings - catalogue paths
        private Dictionary<int, List<string>> prevCatalogues { get; set; }
        //position in catalogue list prev and next catalogue
        private Dictionary<int,int> cataloguePos{get; set;}
        private List<string> undoList;
        private int catPos;
        private List<FileSystemInfo> dirContent { get; set; }
        private bool prevNext { get; set; }

        public Controller()
        {
            this.archivizer = new Archive7Zip();
            this.prevCatalogues = new Dictionary<int, List<string>>();
            prevCatalogues.Add(1, new List<string>());
            prevCatalogues.Add(2, new List<string>());
            this.cataloguePos = new Dictionary<int, int>();
            cataloguePos.Add(1, 0);
            cataloguePos.Add(2, 0);
            this.undoList = new List<string>();
            this.FileSystem = new LocalFileSystem();
            this.Dropbox = new DropBox.DropBox();
            prevNext = false;
        }

        //returns null when there is no more prev folders
        public string prevCatalogue(int windowNumber)
        {
            prevNext = true;
            if (cataloguePos[windowNumber] <=1)
                return null;
            else
            {
                cataloguePos[windowNumber]--;
                return prevCatalogues[windowNumber].ElementAt(cataloguePos[windowNumber]);
            }
        }


        public string nextCatalogue(int windowNumber)
        {
            prevNext = true;
            if (cataloguePos[windowNumber] == prevCatalogues[windowNumber].Count)
                return null;
            else
            {
                cataloguePos[windowNumber]++;
                return prevCatalogues[windowNumber].ElementAt(cataloguePos[windowNumber] - 1);
                
            }
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
        public void orderCopy(List<String> from, string to)
        {
            if (to.StartsWith("www.dropbox.com"))
                foreach (String str in from) FileSystem.copy(str, to);
            else if (to.StartsWith("ftp"))
                foreach (String str in from) SSH.copy(str, to);
            else
            {
                foreach (String str in from) Dropbox.copy(str, to);
            }

        }

        public FileInfo getFileInfo(string path)
        {
            if (path.StartsWith("dropbox.com"))
                return Dropbox.getFile(path);
            else if (path.StartsWith("ftp"))
                return SSH.getFile(path);
            else return FileSystem.getFile(path);

        }

        public List<FileInfo> getFilesInfo(string path)
        {
            if (path.StartsWith("www.dropbox.com"))
                return Dropbox.getFiles(path);
            else if (path.StartsWith("ftp"))
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
                try
                {
                    foreach (DirectoryInfo di in dInfo.GetDirectories())
                    {
                        dInList.Add(di);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return null;
                }
            }
            return dInList;
        }

        //the same as copy
        public void orderMove(List<String> from, string to)
        {
            orderCopy(from, to);
            foreach (String str in from) orderRemove(str);
        }

         public void clearPrevNext(int windowNumber)
        {
            for (int i = 0; i < prevCatalogues[windowNumber].Count; i++)
            {
                if (i > cataloguePos[windowNumber]) prevCatalogues[windowNumber].RemoveAt(i);
            }
        }

        public List<FileSystemInfo> getDirectoryContent(String path, int windowNumber)
        {
            if (prevNext == false)
            {
                this.prevCatalogues[windowNumber].Add(path);
                this.cataloguePos[windowNumber]++;
            }
            dirContent = new List<FileSystemInfo>();
            if (this.getFilesInfo(path) != null)
            foreach (FileInfo fi in this.getFilesInfo(path))
            {
                dirContent.Add(fi);
            }
            if(this.getDirectoryInfo(path) != null)
            foreach (DirectoryInfo di in this.getDirectoryInfo(path))
            {
                dirContent.Add(di);
            }
            prevNext = false;
            return dirContent;
        }

        public void openFile(String path)
        {
            System.Diagnostics.Process.Start(@path);
        }


        public void orderRemove(string path)
        {
            if (path.StartsWith("www.dropbox.com"))
                Dropbox.remove(path);
            else if (path.StartsWith("ftp"))
                SSH.remove(path);
            else FileSystem.remove(path);
        }

        public void orderRename(string path, string name)
        {
            if (path.StartsWith("www.dropbox.com"))
                Dropbox.rename(path, name);
            else if (path.StartsWith("ftp"))
                SSH.rename(path, name);
            else FileSystem.rename(path, name);
        }

        public DriveInfo[] getDrives()
        {
            return DriveInfo.GetDrives();
        }



        public List<Journals.Journal> GetJournals()
        {
            return Journals.GetJournals();
        }

        public void RemoveJournal(int journalNumber)
        {
            Journals.RemoveJournal(journalNumber);
        }


        public void connectSSH(string address, string user, string password)
        {
            SSH = new SSHController(address, user, password);
        }

        public void connectDropbox()
        {
            Dropbox = new DropBox.DropBox();
        }
    }
}
