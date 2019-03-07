using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TestStack.White;
using System.Threading;

namespace tBasicFuncionalityTests
{
    [TestClass]
    public class BasicFunctionality
    {
        private const string BINARY_SERVER_TO_LAUNCH = @"tTCPServer.exe";
        private const string BINARY_CLIENT_TO_LAUNCH = @"tTCPClient.exe";

        private const string WINDOW_SERVER_NAME = "Server TCP";
        private const string WINDOW_CLIENT_NAME = "Client TCP";

        private static string m_PathToServer;
        private static string m_PathToClient;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var applicationDirectory = Directory.GetCurrentDirectory();//path to the binary
            m_PathToServer = Path.Combine(applicationDirectory, BINARY_SERVER_TO_LAUNCH); //initialize path to application
            m_PathToClient = Path.Combine(applicationDirectory, BINARY_CLIENT_TO_LAUNCH); //initialize path to application
        }

        [TestMethod]
        public void SuccessfulStartServer()
        {
            var serverWindow = WindowExtension.GetAppWindow(out Application application, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //start server

            Thread.Sleep(1000);
            
            var expected = "Server started> 127.0.0.1:904\n"; //expected result
            var result = serverWindow.GetTextBoxValue("tbLog"); //actual result

            application.Close(); //close server app

            Assert.AreEqual(expected, result); //evaluate results
        }

        [TestMethod]
        public void SuccessfulStopServer()
        {
            var serverWindow = WindowExtension.GetAppWindow(out Application application, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //start server

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStop");
            Thread.Sleep(1000);

            var expected = "Server stoped"; //expected result
            var result = serverWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            application.Close(); //close server app

            Assert.IsTrue(result); //evaluate results
        }

        [TestMethod]
        public void CantConnectClientToServer()
        {
            var clientWindow = WindowExtension.GetAppWindow(out Application clientApplication, m_PathToClient, WINDOW_CLIENT_NAME); //get client window

            Thread.Sleep(1000);

            clientWindow.ClickButton("btnStart"); //connect to server

            Thread.Sleep(1000);

            var expected = "Cant' connect to the server> 127.0.0.1:904"; //expected result
            var result = clientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            clientApplication.Close(); //close client app

            Assert.IsTrue(result); //evaluate result
        }

        [TestMethod]
        public void SuccessfulConnectToServer()
        {
            //launch server
            var serverWindow = WindowExtension.GetAppWindow(out Application serverApplication, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //start server

            var clientWindow = WindowExtension.GetAppWindow(out Application clientApplication, m_PathToClient, WINDOW_CLIENT_NAME); //get client window

            Thread.Sleep(1000);

            clientWindow.ClickButton("btnStart"); //connect to server

            var expected = "Connected to server> 127.0.0.1:904"; //expected result
            var result = clientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            //close apps
            serverApplication.Close();
            clientApplication.Close();

            Assert.IsTrue(result); //evaluate results
        }

        [TestMethod]
        public void SuccessfulSentMessageToServer()
        {
            //launch server
            var serverWindow = WindowExtension.GetAppWindow(out Application serverApplication, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //perform calculation

            var clientWindow = WindowExtension.GetAppWindow(out Application clientApplication, m_PathToClient, WINDOW_CLIENT_NAME); //get client window

            Thread.Sleep(1000);

            clientWindow.ClickButton("btnStart"); //connect to server

            clientWindow.ClickButton("btnSend"); //send message to server

            Thread.Sleep(1000);

            var expected = "You said> Hello world"; //expected result
            var result = clientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            //close apps
            serverApplication.Close();
            clientApplication.Close();

            Assert.IsTrue(result); //evaluate results
        }

        [TestMethod]
        public void SuccessfulSentMessageToClient()
        {
            //launch server
            var serverWindow = WindowExtension.GetAppWindow(out Application serverApplication, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //perform calculation

            var clientWindow = WindowExtension.GetAppWindow(out Application clientApplication, m_PathToClient, WINDOW_CLIENT_NAME); //get client window

            Thread.Sleep(1000);

            clientWindow.ClickButton("btnStart"); //connect to server

            serverWindow.ClickButton("btnSend"); //send message to server

            Thread.Sleep(1000);

            var expected = "Hello world from server"; //expected result
            var result = clientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            //close apps
            serverApplication.Close();
            clientApplication.Close();

            Assert.IsTrue(result); //evaluate results
        }

        [TestMethod]
        public void SuccessfulSentMessageToTwoClients()
        {
            //launch server
            var serverWindow = WindowExtension.GetAppWindow(out Application serverApplication, m_PathToServer, WINDOW_SERVER_NAME); //get server window

            Thread.Sleep(1000);

            serverWindow.ClickButton("btnStart"); //perform calculation

            var firstClientWindow = WindowExtension.GetAppWindow(out Application firstClientApplication, m_PathToClient, WINDOW_CLIENT_NAME); //get client window
            var secondClientWindow = WindowExtension.GetAppWindow(out Application secondClientApplication, m_PathToClient, WINDOW_CLIENT_NAME);

            Thread.Sleep(1000);

            firstClientWindow.ClickButton("btnStart"); //connect to server
            secondClientWindow.ClickButton("btnStart");

            serverWindow.ClickButton("btnSend"); //send message to server

            Thread.Sleep(1000);

            var expected = "Hello world from server"; //expected result
            var firstResult = firstClientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result
            var secondResult = secondClientWindow.GetTextBoxValue("tbLog").Contains(expected); //actual result

            //close apps
            serverApplication.Close();
            firstClientApplication.Close();
            secondClientApplication.Close();

            Assert.IsTrue(firstResult && secondResult); //evaluate results
        }
    }
}
