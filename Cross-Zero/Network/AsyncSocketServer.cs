using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using CrossZeroCommon;
using Cross_Zero.Logic;

namespace Cross_Zero.Network
{
    public class StateObject
    {
        //Client socket
        public Socket workSocket = null;
        //Size of receive buffer
        public const int bufferSize = 1024;
        //Receive buffer
        public byte[] buffer = new byte[bufferSize];
        //Received data string
        public StringBuilder sb = new StringBuilder();
    }

    public class AsyncSocketServer : INetworkGame
    {
        //Thread signal
        public ManualResetEvent allDone = new ManualResetEvent(false);

        private IPAddress serverAddress;
        private int port;

        public AsyncSocketServer(string ipAddress, string port)
        {
            try
            {
                serverAddress = IPAddress.Parse(ipAddress);
                this.port = int.Parse(port);
            }
            catch (Exception)
            {
                serverAddress = IPAddress.Parse("127.0.0.1");
                this.port = 11000;
                MessageBox.Show(string.Format("IP Address is not correct!\n" +
                                "Server created with default parameters:\n" +
                                "IPAddress: {0}\n" +
                                "port: {1}", "127.0.0.1", 11000));
            }
            
        }

        public void StartListening()
        {
            //Data buffer for incoming data
            byte[] bytes = new byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(serverAddress, port);

            //Create a TCP/IP socket
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                if (ServerCreateComplete != null)
                    ServerCreateComplete("Server", serverAddress.ToString(), port.ToString());

                while (true)
                {
                    // Set the event to nonsignaled state.
                    //allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(AcceptCallback, listener);

                    // Wait until a connection is made before continuing.
                    //allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            //allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.workSocket;

            //Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);

                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, ReadCallback, state);
                }
            }
        }

        private void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSend = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region INetworkGame

        public void StartNetwork()
        {
            StartListening();
        }

        public void SendStartGame(int fieldSize)
        {
        }

        public void SendEnableLine(Vector2 pos, LogicLine.Positioning positioning)
        {
        }

        public event Action<string, string, string> ServerCreateComplete;
        public event Action<string, string, string> ConnectToServerComplete;

        public event Action<int> OnStartGame;
        public event Action<Vector2, LogicLine.Positioning> OnLineEnable;

        #endregion
    }
}
