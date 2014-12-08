using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander._7Zip
{
    interface IArchive7Zip
    {
        void createArchive(List<string> filesToPack, string targetName, string archiveType = "zip", int compressionRate = 0);
        void extractArchive(string archiveName, string destinationPath);

    }
}
