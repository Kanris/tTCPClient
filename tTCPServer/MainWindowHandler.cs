using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleTCP;
using System.Net;
using System.Windows.Controls;
using System.Net.Sockets;

namespace tTCPServer
{
    public class MainWindowHandler
    {
        private MainWindowDataContext m_MainWindowDataContext; //MainWindow.xaml datacontext
        private SimpleTcpServer m_Server; //server reference
        private List<TcpClient> m_Clients; //connected clients;

        //property to return DataContext
        public MainWindowDataContext DataContext
        {
            get
            {
                return m_MainWindowDataContext;
            }
        }

        /// <summary>
        /// Initialize Handler's fields
        /// </summary>
        public MainWindowHandler()
        {
            m_MainWindowDataContext = new MainWindowDataContext(); //initialize data context
            m_Clients = new List<TcpClient>(); //initialize clients list
            InitializeServer(); //initialize server values and events
        }

        /// <summary>
        /// Initialize server values and events
        /// </summary>
        private void InitializeServer()
        {
            m_Server = new SimpleTcpServer(); //create new server instance

            m_Server.Delimiter = 0x13; //enter
            m_Server.StringEncoder = Encoding.UTF8; //message encoder

            m_Server.ClientConnected += Server_ClientConnected; //subscribe to event when client is connected to the server
            m_Server.ClientDisconnected += Server_ClientDisconnected; //subscribe to event when client is disconnected to the server
            m_Server.DataReceived += Server_DataReceived; //subscribe to event when server received message from client
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
        /// Display changes in log textbox
        /// </summary>
        /// <param name="message">Message to display</param>
        public void LogToTextBox(string message)
        {
            /*this.m_tbLog.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() => { m_tbLog.Text += $"{message}\n"; })
                );*/

            DataContext.Log += $"{message}\n"; //display message with data context field
        }

        /// <summary>
        /// Start server
        /// </summary>
        public bool StartServer()
        {
            var isServerStarted = true;

            try
            {
                if (IsValidIP())
                {
                    var iPAddress = IPAddress.Parse(m_MainWindowDataContext.IPAddress); //get ip address
                    m_Server.Start(iPAddress, m_MainWindowDataContext.Port); //try to start server with values

                    LogToTextBox($"Server started> {iPAddress}:{m_MainWindowDataContext.Port}"); //show message that server starts
                }
                else
                {
                    LogToTextBox($"Couldn't start server with this ip address"); //show message that server starts
                    isServerStarted = false;
                }
            }
            catch (Exception ex)
            {
                //display error message in log textbox
                LogToTextBox("Couldn't start server");
                LogToTextBox(ex.Message);

                isServerStarted = false;
            }

            return isServerStarted;
        }

        /// <summary>
        /// Stop Server
        /// </summary>
        public void StopServer()
        {
            if (m_Server.IsStarted)
            {
                m_Server.Stop();
                LogToTextBox($"Server stoped"); //show message that server starts
            }
        }

        /// <summary>
        /// Send message to connected client
        /// </summary>
        public void SendMessageToClient()
        {
            try
            {
                if (m_Clients.Count > 0)
                {
                    var buffer = Encoding.UTF8.GetBytes(DataContext.Message);

                    foreach (var client in m_Clients)
                    {
                        var networkStream = client.GetStream();

                        networkStream.Write(buffer, 0, buffer.Length);
                    }

                    LogToTextBox($"Message was sent to the connected clients.");
                }
                else
                {
                    LogToTextBox($"There is not clients to receive message");
                }
            }
            catch (Exception ex)
            {
                LogToTextBox("Couldn't send message to clients");
                LogToTextBox(ex.Message);
            }
        }

        #region event methods

        /// <summary>
        /// Method that invokes when client diconnects from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">message</param>
        private void Server_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            LogToTextBox($"{(e.Client.LocalEndPoint as IPEndPoint).Address} is disconected.");

            m_Clients.Remove(e);
            LogToTextBox($"Connected clients> {m_Clients.Count}");
        }

        /// <summary>
        /// Method that invokes when client connects to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">message</param>
        private void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            LogToTextBox($"{(e.Client.LocalEndPoint as IPEndPoint).Address} is connected.");

            m_Clients.Add(e); //add client to the list
            LogToTextBox($"Connected clients> {m_Clients.Count}");
        }

        /// <summary>
        /// Method that invokes when server receives message from client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">message</param>
        private void Server_DataReceived(object sender, Message e)
        {
            LogToTextBox($"Received message from {(e.TcpClient.Client.LocalEndPoint as IPEndPoint).Address} > {e.MessageString}");
            e.ReplyLine($"You said> {e.MessageString}");
        }

        #endregion event methods
    }
}
