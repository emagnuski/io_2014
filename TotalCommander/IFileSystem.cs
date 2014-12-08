﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander
{
    interface IFileSystem
    {
        void copy(String from, String to);
        FileInfo getFile(String path);
        List<FileInfo> getFiles(String path);
        void move(String from, String to);
        void remove(String path);
        void rename(String path, String name);
    }
}
