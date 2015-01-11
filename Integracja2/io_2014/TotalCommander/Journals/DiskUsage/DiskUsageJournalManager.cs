using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TotalCommander.Journals.Disk_Usage
{
    public class DiskUsageJournalManager : AbstractJournalManager
    {
        public override void GenerateJournal()
        {
            Dictionary<string, string> atts = new Dictionary<string, string>();
            atts.Add("Number of drives", DriveInfo.GetDrives().Length.ToString());
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    string name = drive.Name;
                    atts.Add(name + " size", drive.TotalSize.ToString());
                    atts.Add(name + " free space", drive.TotalFreeSpace.ToString());
                    atts.Add(name + " filled in", (drive.TotalFreeSpace / (double)drive.TotalSize * 100.0).ToString() + "%");
                }
            }
            journals.Add(new Journal("du-" + DateTime.Now.ToBinary().ToString(), atts));
        }
    }
}
