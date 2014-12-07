using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using LFS;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        private IFileSystem fileSystem = new LocalFileSystem();

        [TestMethod]
        public void TestMethodGetFile()
        {
            string currDir = System.Environment.CurrentDirectory;
            FileInfo expected = new FileInfo(currDir);
            FileInfo result = fileSystem.GetFile(currDir);
            Assert.AreEqual(expected.FullName, result.FullName);
        }

        [TestMethod]
        public void TestMethodGetFiles()
        {
            string currDir = System.Environment.CurrentDirectory;
            string explorer = "C:\\Windows\\explorer.exe";
            DirectoryInfo expected = new DirectoryInfo(currDir);
            List<string> transformedExpectedList = new List<string>();
            foreach (FileInfo finfo in expected.GetFiles())
            {
                transformedExpectedList.Add(finfo.FullName);
            }
            List<string> transformedResultList = new List<string>();
            foreach (FileInfo finfo in fileSystem.GetFiles(currDir))
            {
                transformedResultList.Add(finfo.FullName);
            }
            CollectionAssert.AreEquivalent(transformedExpectedList, transformedResultList);
            transformedExpectedList.Clear();
            transformedExpectedList.Add(explorer);
            transformedResultList.Clear();
            foreach (FileInfo finfo in fileSystem.GetFiles(explorer))
            {
                transformedResultList.Add(finfo.FullName);
            }
            CollectionAssert.AreEqual(transformedExpectedList, transformedResultList);
        }

        [TestMethod]
        public void TestMethodCopyMoveAndDelete()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory);
            }
            Directory.CreateDirectory(tempDirectory);
            string currDir = System.Environment.CurrentDirectory;
            fileSystem.Copy(currDir, tempDirectory, fileSystem);
            DirectoryInfo expected = new DirectoryInfo(currDir);
            DirectoryInfo result = new DirectoryInfo(tempDirectory);
            long expectedSize = 0, resultSize = 0;
            foreach (FileInfo finfo in expected.GetFiles())
            {
                expectedSize += finfo.Length;
            }
            foreach (FileInfo finfo in result.GetFiles())
            {
                resultSize += finfo.Length;
            }
            Assert.AreEqual(expectedSize, resultSize);

            string secondTempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (Directory.Exists(secondTempDirectory))
            {
                Directory.Delete(secondTempDirectory);
            }
            Directory.CreateDirectory(secondTempDirectory);
            fileSystem.Move(tempDirectory, secondTempDirectory, fileSystem);
            expected = new DirectoryInfo(currDir);
            result = new DirectoryInfo(secondTempDirectory);
            expectedSize = 0; resultSize = 0;
            foreach (FileInfo finfo in expected.GetFiles())
            {
                expectedSize += finfo.Length;
            }
            foreach (FileInfo finfo in result.GetFiles())
            {
                resultSize += finfo.Length;
            }
            Assert.AreEqual(expectedSize, resultSize);
            Assert.IsFalse(Directory.Exists(tempDirectory));

            fileSystem.Remove(secondTempDirectory);
            Assert.IsFalse(Directory.Exists(secondTempDirectory));
        }
    }
}
