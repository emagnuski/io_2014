using System;
using System.Collections.Generic;
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
//using TotalCommander.Controller;

namespace TotalCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //IController contr = new Controller.Controller(); 

        public MainWindow()
        {
            InitializeComponent();
            
            setSsh.Checked += this.fsetSsh;
            setDrop.Checked += this.fsetDropbox;
            setLok.Checked += this.fsetLokalny;
            sendButton.Click += this.fsendButton;

            menuExit.Click += this.fexitButton;
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
