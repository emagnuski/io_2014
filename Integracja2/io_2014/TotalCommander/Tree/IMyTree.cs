using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TotalCommander.Tree
{
    interface IMyTree
    {

        List<TreeViewItem> createTreeView(String pattern = null);

        void getDirs(TreeViewItem parentItem, String pattern = null);
    }
}
