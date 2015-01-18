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
using System.Collections.ObjectModel;
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
        bool debug = true;

        public MainWindow()
        {
            InitializeComponent();
            setSsh.Checked += this.fsetSsh;
            setDrop.Checked += this.fsetDropbox;
            setLok.Checked += this.fsetLokalny;
            sendButton.Click += this.fsendButton;
            FilterSubmit.Click += this.FilterSubmitted;
            menuExit.Click += this.fexitButton;
            menuAbout.Click += this.faboutButton;
            GenerateStat.Click += this.GenerateStatictics;
            MakeArchive.Click += this.GenerateArchive;


            //na poczatek powinno sie ustawić lokalny po obu stronach
            // ps. powinien byc sam ukosnik i powinno zwracac liste dyskow
            // ale wtedy probuje zalaczyc się ssh czy cos, dlatego wpisalem sztywno
            // no i powinno to przechodzic jakos przez filtrowanie i sortowanie
            // aha, no i jeszcze - jak na razie kontroler zwraca tylko pliki, bez katalogow
            // nie wiem czemu
            setLeftView(contr.getFilesInfo("D:/"));
            setRightView(contr.getFilesInfo("C:/"));

        }

        public void fsetLokalny(object sender, RoutedEventArgs e)
        {
            login.IsEnabled = false;
            haslo.IsEnabled = false;
            server.IsEnabled = false;
            port.IsEnabled = false;
            if (debug) Console.Out.WriteLine("Selected local file system");
        }

        public void fsetDropbox(object sender, RoutedEventArgs e)
        {
            login.IsEnabled = true;
            haslo.IsEnabled = true;
            server.IsEnabled = false;
            port.IsEnabled = false;
            if (debug) Console.Out.WriteLine("Selected Dropbox");
        }

        public void fsetSsh(Object sender, RoutedEventArgs e)
        {
            login.IsEnabled = true;
            haslo.IsEnabled = true;
            server.IsEnabled = true;
            port.IsEnabled = true;
            if (debug) Console.Out.WriteLine("Selected SSH");
        }


        //wykonywany po wcisnieciu przycisku wyslij
        //z niego mozna oczytac dane potrzebne do polaczenia
        public void fsendButton(Object sender, RoutedEventArgs e)
        {
            if (debug) Console.Out.WriteLine("Login data server: " + server.Text+" port: "+port.Text+" login: "+login.Text+" pass: ****");
            //wyslanie danych logowania do kontrolera
            if ((bool)setLok.IsChecked)
            {
                //wybrano system lokalny

                //setLeftView(contr.connectLocal());
            }
            else if ((bool)setSsh.IsChecked)
            {
                //wybrano SSH

                //setLeftView(contr.connectSSH(server.Text, port.Text, login.Text, haslo.Text));

            }
            else if ((bool)setDrop.IsChecked)
            {
                //wybrano DROPBOX

                //setLeftView(contr.connectDropbox(login.Text, haslo.Text));
            }
        }

        //dla lewego okna
        //funkcja pobiera jako argument listę 
        //objektow FileInfo i wyswietla ja w oknie programu
        public void setLeftView(List<FileInfo> files)
        {
            LeftTree.Items.Clear();
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
                
                LeftTree.Items.Add(item);
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

        //dla prawego okna
        //funkcja pobiera jako argument listę 
        //objektow FileInfo i wyswietla ja w oknie programu
        public void setRightView(List<FileInfo> files)
        {
            RightTree.Items.Clear();
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

                RightTree.Items.Add(item);
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

        void FilterSubmitted(object sender, EventArgs e)
        {
            String sortby = ((ComboBoxItem)SortBySelection.SelectedItem).Content.ToString();
            String sortascdsc = ((ComboBoxItem)SortByAscDsc.SelectedItem).Content.ToString();
            String filename = FileNameContains.Text;
            String extension = FileExtension.Text;

            if (debug) Console.Out.WriteLine("Sort: " + sortby + " " + sortascdsc + " filter filename: " + filename + " extension: " + extension);
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

        void addToInfoBox(String txt)
        {
            InfoBox.Content += "\n\r"+txt;
        }

        public void faboutButton(object sender, RoutedEventArgs e)
        {
            string message = "Przed swymi oczyma masz właśnie jeden z najbardziej zaawansowanych managerów plików dla systemu Windows. Powstał on w ramach prac badawczych, których największym osiągnięciem było poprawne zaprogramowanie przycisku 'Exit', który możesz podziwiać po przejściu do menu 'File'.";
            string title = "About";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void fexitButton(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Niestety tamten przycisk nie działa, naciśnij OK, aby zamknąć", "To prawie koniec", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (res)
            {
                case MessageBoxResult.OK: this.Close(); break;
                default: return;
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
