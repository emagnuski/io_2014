using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TotalCommander.Journals;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace JournalUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestThreadStarting()
        {
            AbstractJournalManager jmanager = new TotalCommander.Journals.Disk_Usage.DiskUsageJournalManager();
            jmanager.GenerationInterval = 500;
            Thread t = jmanager.Start();
            Thread.Sleep(550);
            t.Abort();
            Assert.AreEqual(2, jmanager.GetJournals().Count);
        }
        [TestMethod]
        public void TestDiskUsage()
        {
            AbstractJournalManager jmanager = new TotalCommander.Journals.Disk_Usage.DiskUsageJournalManager();
            jmanager.GenerateJournal();
            List<Journal> journals = jmanager.GetJournals();
            Assert.AreNotEqual(0, journals.Count);
            journals[0].Serialize(Path.GetTempPath());
        }
    }
}
