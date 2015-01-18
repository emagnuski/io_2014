using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander._7Zip
{
    public class Archive7Zip : IArchive7Zip
    {
        private StringBuilder command { get; set; }
        private string archiveType{get; set;}
        private int compressionRate{get; set;}





        private void startProcess()
        {
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "7za.exe";

            p.Arguments = command.ToString();
            p.WindowStyle = ProcessWindowStyle.Hidden;
            Process x = Process.Start(p);
            x.WaitForExit();
        }


        //a(archive or add) could be also used to add elements to existing archive
        public void createArchive(List<string> filesToPack, string targetName, string archiveType, int compressionRate)
        {
            this.command = new StringBuilder();
            this.archiveType = archiveType;
            this.compressionRate = compressionRate;

            archiveType = "zip";
            compressionRate = 9;

            this.command.Append("a -t").Append(archiveType).Append(" \"").Append(targetName);
            
            foreach(string fileName in filesToPack){
            this.command.Append("\" \"").Append(fileName).Append("\" ");
            }
                 this.command.Append("-mx=").Append(compressionRate.ToString());

            startProcess();

        }

        public void extractArchive(string archiveName, string destinationPath)
        {
            this.command = new StringBuilder();

            this.command.Append("e \"").Append(archiveName).Append("\" -o").Append(destinationPath).Append(" -r");

            startProcess();
        }
    }
}
