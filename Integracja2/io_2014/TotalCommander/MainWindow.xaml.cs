using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TotalCommander.Controller;
using TotalCommander.Tree;
//using TotalCommander.Controller;

namespace TotalCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder pathBuilder = new StringBuilder();
        IController contr = new Controller.Controller();
        IMyTree mytree = new MyTree();
        TreeViewItem selectedNode;
        public MainWindow()
        {
            InitializeComponent();
            setSsh.Checked += this.fsetSsh;
            setDrop.Checked += this.fsetDropbox;
            setLok.Checked += this.fsetLokalny;
            sendButton.Click += this.fsendButton;
            menuExit.Click += this.fexitButton;


            
            this.generateView(tree);

        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
    {

        //if (sender is TreeViewItem)
        //{
        //    if (!((TreeViewItem)sender).IsSelected)
        //    {
        //        return;
        //    }
        //}

        //this.pathBuilder.Clear();
        //selectedNode = (TreeViewItem)args.Source;
        //Console.WriteLine(selectedNode.Header);
        //TreeViewItem parent = selectedNode.Parent as TreeViewItem;
        //if (parent != null && parent.Parent != tree)
        //{
        //    this.pathBuilder.Append(parent.Header);
        //    parent = parent.Parent as TreeViewItem;
        //}
        //this.pathBuilder.Append(selectedNode.Header);
        //Console.WriteLine(pathBuilder.ToString());
        //foreach (FileInfo fi in contr.getFilesInfo(pathBuilder.ToString()))
        //{
        //    selectedNode.Items.Add(fi.ToString());
        //}
        //foreach (DirectoryInfo dinfo in contr.getDirectoryInfo(pathBuilder.ToString()))
        //{
        //    selectedNode.Items.Add(dinfo.Name);
        //}
    }
        //nie działa jak powinno
        private void treeExpanded(object sender, RoutedEventArgs e)
        {

            TreeViewItem tvitem = e.OriginalSource as TreeViewItem;
            TreeViewItem parent = tvitem.Parent as TreeViewItem;
            //foreach (TreeViewItem tvchild in tvitem.Items)
            this.pathBuilder.Clear();
            //parent = (tvitem.Items.GetItemAt(i) as TreeViewItem);
            this.pathBuilder.Append(tvitem.Header);
            //this.pathBuilder.Append(tvitem.Items.GetItemAt(i).ToString());
            if (parent != null)
            {
                if (tvitem.Parent is TreeView)
                    Console.WriteLine(tvitem.Parent);
            }
            foreach (FileInfo fi in contr.getFilesInfo(pathBuilder.ToString()))
            {
                tvitem.Items.Add(fi.ToString());
            }
            foreach (DirectoryInfo dinfo in contr.getDirectoryInfo(pathBuilder.ToString()))
            {
                tvitem.Items.Add(dinfo.Name);
            }
        }

        private void generateView(TreeView tree)
       {
           foreach (TreeViewItem tvi in mytree.createTreeView())
           {
               this.tree.Items.Add(tvi);
           }
       }

        public void fsetLokalny(object sender, RoutedEventArgs e)
        {
            login.IsEnabled = false;
            haslo.IsEnabled = false;
            server.IsEnabled = false;
            port.IsEnabled = false;
        }

        public void fsetDropbox(object sender, RoutedEventArgs e)
        {
            login.IsEnabled = true;
            haslo.IsEnabled = true;
            server.IsEnabled = false;
            port.IsEnabled = false;
        }

        public void fsetSsh(Object sender, RoutedEventArgs e)
        {
            login.IsEnabled = true;
            haslo.IsEnabled = true;
            server.IsEnabled = true;
            port.IsEnabled = true;
        }

        public void fsendButton(Object sender, RoutedEventArgs e)
        {
            //wyslanie danych logowania do kontrolera
            //contr.loginData()
        }

        public void fexitButton(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
