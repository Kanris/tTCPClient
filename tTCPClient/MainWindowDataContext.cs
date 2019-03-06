using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tTCPClient
{
    public class MainWindowDataContext : INotifyPropertyChanged
    {
        #region fields

        private string m_IPAddress = "127.0.0.1"; //ip address
        private int m_Port = 904; //port
        private string m_Message = "Hello world";
        private string m_Log = string.Empty; //log message

        #endregion

        #region properties

        public string IPAddress 
        {
            set
            {
                m_IPAddress = value;
                OnPropertyChanged(); //notify window that property changed
            }
            get
            {
                return m_IPAddress;
            }
        } 

        public int Port
        {
            set
            {
                m_Port = value;
                OnPropertyChanged(); //notify window that property changed
            }
            get
            {
                return m_Port;
            }
        }

        public string Log
        {
            set
            {
                m_Log = value;
                OnPropertyChanged(); //notify window that property changed
            }
            get
            {
                return m_Log;
            }
        }

        public string Message
        {
            set
            {
                m_Message = value;
                OnPropertyChanged(); //notify window that property changed
            }
            get
            {
                return m_Message;
            }
        }

        #endregion properties

        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion events

        #region methods

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion methods
    }
}
