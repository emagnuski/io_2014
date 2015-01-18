using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TotalCommander.Tree
{
    class MyTree : IMyTree
    {
        //TreeViewItem item;
        List<TreeViewItem> items = new List<TreeViewItem>();

        public MyTree()
        {
        }


        private void getDrives(TreeView tree)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                TreeViewItem treeitem = new TreeViewItem() { Header = drive.Name };
                tree.Items.Add(treeitem);
                }
            
        }


        public List<TreeViewItem> createTreeView(String pattern = null)
        {
            //item = new TreeViewItem(); // Root Item
            //item.Header = "\\";

            try
            {

                foreach (DriveInfo d in DriveInfo.GetDrives())
                {
                    TreeViewItem dysk = new TreeViewItem(){ Header = d.Name };
                    items.Add(dysk);
                    try
                    {
                        //getDirs(dysk);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                    }
                }
                // Tymczasowe do podanego dysku

                //TreeViewItem dy = new TreeViewItem();
                //dy.Header = "D:\\";
                //item.Items.Add(dy);
                //getDirs(dy, pattern);
                

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return items;
            //drzewko.Items.Add(item);
        }

        public void getDirs(TreeViewItem parentItem, String pattern = null)
        {
            try
            {
                string[] dirs;
                if (pattern == null)
                    dirs = Directory.GetDirectories(parentItem.Header.ToString()); // Listowanie struktury w podanym katalogu
                else
                    dirs = Directory.GetDirectories(parentItem.Header.ToString(), pattern); // Listowanie struktury w podanym katalogu po patternie

                if (dirs.Length > 0)
                {
                    foreach (string dir in dirs)
                    {
                        TreeViewItem item = new TreeViewItem();
                        TreeViewItem empty = new TreeViewItem();
                        empty.Header = "";
                        item.Header = dir;
                        item.Items.Add(empty);
                        parentItem.Items.Add(item);
                        //getDirs(item);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        //private void findItems(object sender, RoutedEventArgs e)
        //{
        //    drzewko.Items.Clear();
        //    createTreeView(wyszukiwarka.Text);
        //}

        //private void showWholeTreeView(object sender, RoutedEventArgs e)
        //{
        //    drzewko.Items.Clear();
        //    createTreeView();
        //}

        //public TreeViewItem getItem()
        //{
        //    return item;
        //}
    }
}
