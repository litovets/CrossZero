using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CrossZeroCommon;
using Cross_Zero.Logic;

namespace Cross_Zero.Network
{
    public class AsyncSocketClient : INetworkGame
    {
        // The port number for the remote device.
        private int port = 11000;
        private IPAddress serverAddress;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.
        private static String response = String.Empty;

        private Socket client;
        private NetworkStream netStream;

        public AsyncSocketClient(string ipAddress, string port)
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
                                "Try to connect with default parameters:\n" +
                                "IPAddress: {0}\n" +
                                "port: {1}", "127.0.0.1", 11000));
            }
        }

        private void StartClient()
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // The name of the 
                // remote device is "host.contoso.com".
                IPAddress ipAddress = serverAddress;//IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                //Create network stream
                netStream = new NetworkStream(client);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP, ConnectCallback, client);
                //connectDone.WaitOne();

                // Send test data to the remote device.
                //Send(server, "This is a test<EOF>");
                //sendDone.WaitOne();

                // Receive the response from the remote device.
                //Receive(server);
                //receiveDone.WaitOne();

                // Write the response to the console.
                //Console.WriteLine("Response received : {0}", response);

                // Release the socket.
                //server.Shutdown(SocketShutdown.Both);
                //server.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                //Socket server = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint);

                // Signal that the connection has been made.
                //connectDone.Set();

                // Receive the response from the remote device.
                Receive(client);
                //receiveDone.WaitOne();
                
                // Send test data to the remote device.
                Send(client, "This is a test<EOF>");
                //sendDone.WaitOne();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Receive(Socket server)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = server;

                // Begin receiving the data from the remote device.
                netStream.BeginRead(state.buffer, 0, state.buffer.Length, ReceiveCallback, state);
                //server.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                client = state.workSocket;

                // Read data from the remote device.
                //int bytesRead = client.EndReceive(ar);
                int bytesRead = netStream.EndRead(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, ReceiveCallback, state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    //receiveDone.Set();
                }
                client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, ReceiveCallback, state);
                //receiveDone.WaitOne();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private void Send(Socket client, byte[] data)
        {
            client.BeginSend(data, 0, data.Length, 0, SendCallback, client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                //sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StartNetwork()
        {
            StartClient();
        }

        public void SendStartGame(int fieldSize)
        {
            int[] netData = {(int) NetworkCode.StartGame, fieldSize};
            byte[] buffer = new byte[netData.Length * 4];
            Buffer.BlockCopy(netData, 0, buffer, 0, buffer.Length);
            Send(client, buffer);

        }

        public void SendEnableLine(Vector2 pos, Logic.LogicLine.Positioning positioning)
        {
            int[] netData = {(int)NetworkCode.EnableLine, pos.X, pos.Y, (int) positioning};
            byte[] buffer = new byte[netData.Length * 4];
            Buffer.BlockCopy(netData, 0, buffer, 0, buffer.Length);
            Send(client, buffer);
        }

        public event Action<string, string, string> ServerCreateComplete;
        public event Action<string, string, string> ConnectToServerComplete;

        public event Action<int> OnStartGame;
        public event Action<Vector2, LogicLine.Positioning> OnLineEnable;
    }
}
