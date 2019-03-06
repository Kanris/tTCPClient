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
using System.Net;

namespace tTCPServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowHandler m_MainWindowHandler; //contains method to perform for MainWindowXaml

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_MainWindowHandler = new MainWindowHandler(); //initialize handler
            this.DataContext = m_MainWindowHandler.DataContext; //set data context for MainWindow.xaml
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            m_MainWindowHandler.StartServer(); //start server            
            SetActiveStartButton(false);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            m_MainWindowHandler.StopServer(); //stop server            
            SetActiveStartButton(true);
        }

        private void SetActiveStartButton(bool value)
        {
            btnStop.IsEnabled = !value;
            btnSend.IsEnabled = !value;
            btnStart.IsEnabled = value;
        }
    }
}
