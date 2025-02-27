﻿using System;
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

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var isCreated = await m_MainWindowHandler.StartServer(); //start server   
            
            if (isCreated)
                SetActiveStartButton(false);
        }

        private async void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            await m_MainWindowHandler.StopServer(); //stop server            
            SetActiveStartButton(true);
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await m_MainWindowHandler.SendMessageToClient();
        }

        private void SetActiveStartButton(bool value)
        {
            btnStop.IsEnabled = !value;
            btnSend.IsEnabled = !value;
            btnStart.IsEnabled = value;
        }

        /// <summary>
        /// Only character allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbIpAddress.Text = new string(tbIpAddress.Text.Where(x => Char.IsDigit(x) || x == '.').ToArray());
        }
    }
}
