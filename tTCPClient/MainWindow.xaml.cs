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
using SimpleTCP;

namespace tTCPClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowHandler m_MainWindowHandler; //MainWindow methods

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_MainWindowHandler = new MainWindowHandler(); //initialize maindinow handler
            this.DataContext = m_MainWindowHandler.DataContext; //set data context
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            var isConnected = await m_MainWindowHandler.ConnectToServer(); //connect to server

            btnSend.IsEnabled = isConnected; //enable/disable button base on client is connected or not
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await m_MainWindowHandler.SendMessage(); //send message to server
        }

        private void TbIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbIpAddress.Text = new string(tbIpAddress.Text.Where(x => Char.IsDigit(x) || x == '.').ToArray());
        }
    }
}
