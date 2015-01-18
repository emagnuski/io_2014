using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int activeTree { get; set; }
        IController contr = new Controller.Controller();
        IMyTree mytree = new MyTree();
        TreeViewItem selectedNode;
        ObservableCollection<FileSystemInfo> oInfo { get; set; }
        ObservableCollection<FileSystemInfo> oInfo2 { get; set; }
        ObservableCollection<DriveInfo> driveInfo { get; set; }
        bool debug = true;
        List<String> stringi { get; set; }
        private string directoryPath { get; set; }

        public MainWindow()
        {
            oInfo = new ObservableCollection<FileSystemInfo>();
            oInfo2 = new ObservableCollection<FileSystemInfo>();
            driveInfo = new ObservableCollection<DriveInfo>();
            this.DataContext = new
            {
                fInfo = oInfo,
                drInfo = driveInfo,
                fInfo2 = oInfo2,
            };

            InitializeComponent();
            setSsh.Checked += this.fsetSsh;
            setDrop.Checked += this.fsetDropbox;
            setLok.Checked += this.fsetLokalny;
            sendButton.Click += this.fsendButton;
            menuExit.Click += this.fexitButton;

            GenerateStat.Click += this.GenerateStatictics;
            MakeArchive.Click += this.GenerateArchive;
            getDrives();

        }

        private void OnItemMouseDoubleClick1(object sender, MouseButtonEventArgs args)
    {
        if (sender is TreeViewItem)
        {
            if (!((TreeViewItem)sender).IsSelected)
            {
                return;
            }
        }
        selectedNode = (TreeViewItem)args.Source;
        showDir(selectedNode, tree1);
        
    }
        private void OnItemMouseDoubleClick2(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }
            selectedNode = (TreeViewItem)args.Source;
            showDir(selectedNode, tree2);

        }




        private void showDir(TreeViewItem selectedNode, TreeView tv)
        {
            if (selectedNode.Header.GetType().Equals(typeof(DirectoryInfo)))
            {

                directoryPath =
                    (selectedNode.Header as DirectoryInfo).FullName;
                if (tv.Name.Equals("tree1"))
                    contr.clearPrevNext(1);
                else contr.clearPrevNext(2);
                getdirContent((selectedNode.Header as DirectoryInfo).FullName, tv);

            }
            else if (selectedNode.Header.GetType().Equals(typeof(FileInfo)))
            {

                if (selectedNode.Header.ToString().Equals(".."))
                {
                    int indeks;
                    bool rem = false;
                    if (directoryPath != null)
                    {
                        indeks = 0;
                        if (directoryPath.LastIndexOf("\\") == -1) return;
                        else
                            indeks = directoryPath.LastIndexOf("\\");
                        directoryPath = directoryPath.Remove(indeks);

                        if (directoryPath.LastIndexOf("\\") == -1)
                        {
                            directoryPath = directoryPath + "\\";
                            rem = true;
                        }
                        getdirContent(directoryPath, tv);

                        if (rem == true) directoryPath.Remove(indeks);
                    }
                }
                else
                {
                    contr.openFile((selectedNode.Header as FileInfo).FullName);
                }
            }
        }


        /// <summary>
        /// Jak dobrze rozumiem tu są robione rzeczy dotyczące pokazywania drzewek
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tv"></param>
        private void getdirContent(String path, TreeView tv)
        {
            if (path == null) return;
            if (tv.Name.Equals("tree1"))
                if (contr.getDirectoryContent(path, 1).Count != 0)
                {
                    this.oInfo.Clear();
                    oInfo.Add(new FileInfo(".."));
                    foreach (FileSystemInfo fis in contr.getDirectoryContent(path, 2))
                    {
                        oInfo.Add(fis);
     
                    }
                    activeTree = 1;
                }
                if (tv.Name.Equals("tree2"))
                    if (contr.getDirectoryContent(path, 1).Count != 0)
                    {
                        this.oInfo2.Clear();
                        oInfo2.Add(new FileInfo(".."));
                        foreach (FileSystemInfo fis in contr.getDirectoryContent(path, 2))
                        {
                            oInfo2.Add(fis);
                        }
                        activeTree = 2;
                    }

        }


        public void getDrives()
        {
            this.driveInfo.Clear();
            foreach (DriveInfo di in contr.getDrives())
            {
                driveInfo.Add(di);
            }
            combo1.SelectedIndex = combo1.Items.IndexOf(driveInfo.ElementAt(0));
            combo2.SelectedIndex = combo2.Items.IndexOf(driveInfo.ElementAt(0));
        }





        private void treeExpanded(object sender, RoutedEventArgs e)
        {

            //TreeViewItem tvi = e.OriginalSource as TreeViewItem;

            //if (tvi.Header.GetType().Equals(typeof(DirectoryInfo)))
            //    Console.WriteLine("line 2");
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

        private void combo1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getdirContent((sender as ComboBox).SelectedValue.ToString(), tree1);
        }

        private void combo2SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getdirContent((sender as ComboBox).SelectedValue.ToString(), tree2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (activeTree == 1)
            {
                    getdirContent(contr.prevCatalogue(1), tree1);
            }
            else if (activeTree == 2)
                getdirContent(contr.prevCatalogue(2), tree2);


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (activeTree == 1)
            {
                Console.WriteLine(contr.nextCatalogue(1) + " prev");
                getdirContent(contr.nextCatalogue(1), tree1);

                //contr.nextCatalogue(1);
            }
            else if (activeTree == 2)
                getdirContent(contr.nextCatalogue(2), tree2);
        }

        //ZABAWECZKI EMILA

        /// <summary>
        /// Metoda, która reaguje na naciśnięcie przycisku wyszukiwania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickedSearchButton(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Chce wyszukiwać, a to nie działa, bo Rokita nie zrobił XD");
        }


        void GenerateStatictics(object sender, RoutedEventArgs e)
        {
            //clicked stat generate button
        }

        void GenerateArchive(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "archive";
            dlg.DefaultExt = ".zip";
            dlg.Filter = "Text documents (.zip)|*.zip";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;//jak zapisac
                MessageBox.Show("To i tak się nie uda", "Nie robimy archiwum", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                MessageBox.Show("Nie to nie!", "Nie robimy archiwum", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //dla lewego okna
        //funkcja pobiera jako argument listę 
        //objektow FileInfo i wyswietla ja w oknie programu
        public void setLeftView(List<FileInfo> files)
        {
            tree1.Items.Clear();
            files.ForEach(delegate(FileInfo filei)
            {
                var item = new TreeViewItem();
                item = GetTreeView(item.Uid, filei.Name, "resources/" + getImgForExt(filei.Extension));

                ContextMenu menu = new ContextMenu();
                MenuItem iCopy = new MenuItem();
                iCopy.Header = "Copy";
                iCopy.Name = "icopy";
                MenuItem iCut = new MenuItem();
                iCut.Header = "Cut";
                iCut.Name = "icut";
                MenuItem iPaste = new MenuItem();
                iPaste.Header = "Paste";
                iPaste.Name = "ipaste";
                menu.Items.Add(iCopy);
                menu.Items.Add(iCut);
                menu.Items.Add(iPaste);
                item.ContextMenu = menu;

                //item.Header = filei.Name;
                item.MouseLeftButtonUp += item_MouseLeftButtonUp_leftTree;
                item.MouseDoubleClick += item_MouseDoubleClick_leftTree;

                tree1.Items.Add(item);
            });
        }

        //dla lewego okna
        void item_MouseDoubleClick_leftTree(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as TreeViewItem;
            if (obj is TreeViewItem)
            {
                //podwojnie kliknieto na element
                // obj.Header - nazwa pliku/foleru kliknietego
                //tutaj powinniśmy wywoływać u kontrolera metodę, która przenosiła
                //do katalogu wyżej i zwracała zawartość
                //coś takiego: setLeftView(contr.nextCatalogue(0, obj.Header));
                if (debug) Console.Out.WriteLine("DoubleClicked " + obj.Header);
            }
        }

        //dla lewego okna
        void item_MouseLeftButtonUp_leftTree(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as TreeViewItem;
            if (obj is TreeViewItem)
            {
                //pojedynczo kliknieto na element
                // obj.Header - nazwa pliku/foleru kliknietego
                //tutaj przekazanie kontrolerowi informacji o selecji
                //albo deselekcji pliku/katalogu aby dodał do listy
                if (debug) Console.Out.WriteLine("Clicked " + obj.Header);
            }
        }

        //pomocnicza funkcja do programowego dodawania ikony
        private TreeViewItem GetTreeView(string uid, string text, string imagePath)
        {
            TreeViewItem item = new TreeViewItem();
            item.Uid = uid;
            item.IsExpanded = false;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            // create Image
            Image image = new Image();
            image.Source = new BitmapImage
                (new Uri("pack://application:,,/" + imagePath));
            image.Width = 16;
            image.Height = 16;
            // Label
            Label lbl = new Label();
            lbl.Content = text;


            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            return item;
        }

        //dla prawego okna
        //funkcja pobiera jako argument listę 
        //objektow FileInfo i wyswietla ja w oknie programu
        public void setRightView(List<FileInfo> files)
        {
            tree2.Items.Clear();
            files.ForEach(delegate(FileInfo filei)
            {
                var item = new TreeViewItem();
                item = GetTreeView(item.Uid, filei.Name, "resources/" + getImgForExt(filei.Extension));

                ContextMenu menu = new ContextMenu();
                MenuItem iCopy = new MenuItem();
                iCopy.Header = "Copy";
                iCopy.Name = "icopy";
                MenuItem iCut = new MenuItem();
                iCut.Header = "Cut";
                iCut.Name = "icut";
                MenuItem iPaste = new MenuItem();
                iPaste.Header = "Paste";
                iPaste.Name = "ipaste";
                menu.Items.Add(iCopy);
                menu.Items.Add(iCut);
                menu.Items.Add(iPaste);
                item.ContextMenu = menu;

                //item.Header = filei.Name;
                item.MouseLeftButtonUp += item_MouseLeftButtonUp_leftTree;
                item.MouseDoubleClick += item_MouseDoubleClick_leftTree;

                tree2.Items.Add(item);
            });
        }

        //dla prawego okna
        void item_MouseDoubleClick_rightTree(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as TreeViewItem;
            if (obj is TreeViewItem)
            {
                //podwojnie kliknieto na element
                // obj.Header - nazwa pliku/foleru kliknietego
                //tutaj powinniśmy wywoływać u kontrolera metodę, która przenosiła
                //do katalogu wyżej i zwracała zawartość
                //coś takiego: setLeftView(contr.nextCatalogue(0, obj.Header));
                if (debug) Console.Out.WriteLine("DoubleClicked " + obj.Header);
            }
        }

        //dla prawego okna
        void item_MouseLeftButtonUp_rightTree(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as TreeViewItem;
            if (obj is TreeViewItem)
            {
                //pojedynczo kliknieto na element
                // obj.Header - nazwa pliku/foleru kliknietego
                //tutaj przekazanie kontrolerowi informacji o selecji
                //albo deselekcji pliku/katalogu aby dodał do listy
                if (debug) Console.Out.WriteLine("Clicked " + obj.Header);
            }
        }

        string getImgForExt(string ext)
        {
            string graph = "text.png";
            switch (ext)
            {
                case ".txt": graph = "text.png"; break;
                case ".jpg":
                case ".png":
                case ".gif":
                case ".jpeg":
                case ".bmp":
                case ".icon": graph = "photo.png"; break;
                case ".zip":
                case ".rar":
                case ".7z":
                case ".tar":
                case ".gz": graph = "archive.png"; break;
                case ".mp3":
                case ".ogg":
                case ".wmv":
                case ".flac":
                case ".acc":
                case ".amr": graph = "music.png"; break;
                case ".lnk": graph = "link.png"; break;
                case ".sys":
                case ".bat":
                case ".exe":
                case ".dll":
                case ".ini":
                case ".bin":
                case ".cab":
                case ".msi": graph = "setting.png"; break;
            }
            return graph;
        }
    }
}
