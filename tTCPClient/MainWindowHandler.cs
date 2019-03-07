using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleTCP;

namespace tTCPClient
{
    public class MainWindowHandler
    {
        private MainWindowDataContext m_DataContext; //data context
        private SimpleTcpClient m_Client; //tcp client

        /// <summary>
        /// return MainWindow DataContext
        /// </summary>
        public MainWindowDataContext DataContext
        {
            get
            {
                return m_DataContext;
            }
        }

        /// <summary>
        /// Initialize Handler's fields
        /// </summary>
        public MainWindowHandler()
        {
            m_DataContext = new MainWindowDataContext(); // initialize data context
            InitializeClient(); //initialize client
        }

        /// <summary>
        /// Initialize client
        /// </summary>
        private void InitializeClient()
        {
            m_Client = new SimpleTcpClient(); //create client instance
            m_Client.StringEncoder = Encoding.UTF8; //set message encoder
            m_Client.DataReceived += Client_DataReceived; //subscribe to event when client received message
        }

        /// <summary>
        /// Check is DataContext.IPAddress is valid IPAddress
        /// </summary>
        /// <returns>True if IPAddress is valid and false if IPAddress is not valid</returns>
        private bool IsValidIP()
        {
            var pattern = @"^([0-9]{1,3}\.){3}([0-9]{1,3})$";

            return Regex.IsMatch(DataContext.IPAddress, pattern);
        }

        /// <summary>
        /// Write message to log text box
        /// </summary>
        /// <param name="message"></param>
        public void LogToTextBox(string message)
        {
            /*this.tbLog.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() => { tbLog.Text += $"{message}\n"; })
            );*/

            m_DataContext.Log += $"{message}\n"; 
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectToServer()
        {
            var isConnected = true; //indicates if client is connect to server

            await Task.Run(() =>
            {
                try
                {
                    if (IsValidIP())
                    {
                        m_Client.Connect(m_DataContext.IPAddress, m_DataContext.Port); //try to connect to the server
                        LogToTextBox($"Connected to server> {m_DataContext.IPAddress}:{m_DataContext.Port}"); //display connect info
                    }
                    else
                    {
                        LogToTextBox("Couldn't connect to server with this IP address.");
                    }
                }
                catch (Exception ex)
                {
                    //display connection error
                    LogToTextBox($"Cant' connect to the server> {m_DataContext.IPAddress}:{m_DataContext.Port}");
                    LogToTextBox(ex.Message);

                    isConnected = false; //indicate that client could connect to server
                }

            });

            return isConnected;
        }

        /// <summary>
        /// Send text message to server
        /// </summary>
        public async Task SendMessage()
        {
            await Task.Run(() =>
            {
               try
               {
                   m_Client.WriteLineAndGetReply(m_DataContext.Message, TimeSpan.FromSeconds(3)); //send message to server
               }
               catch (Exception ex)
               {
                   //display send message error
                   LogToTextBox("Couldn't send message to the server!");
                   LogToTextBox(ex.Message);
               }
            });
        }

        /// <summary>
        /// Event that rised when client receives message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DataReceived(object sender, Message e)
        {
            //display message in log text box
            LogToTextBox(e.MessageString);
        }
    }
}
