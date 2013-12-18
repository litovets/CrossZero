using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CrossZeroCommon;
using Cross_Zero.Logic;

namespace Cross_Zero.Network
{
    public class NetworkManager
    {
        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NetworkManager();
                    return _instance;
                }
                return _instance;
            }
        }

        private NetworkManager() {}

        private static NetworkManager _instance;

        public bool IsMultiplayerGame { get; set; }
        public bool IsServer { get; set; }
        public INetworkGame NetworkGame { get; private set; }

        public event Action<string, string, string> ServerIsCreated;
        public event Action<string, string, string> ClientIsConnected;
        public event Action ClientDisconnected;
        public event Action<int> StartGameEvent;


        public string GetHostAddress()
        {
            IPAddress[] addrList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress ipAddress in addrList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    return ipAddress.ToString();
            }
            return addrList[0].ToString();
        }

        public void StartServer(string ipAddress, string port)
        {
            NetworkGame = new AsyncSocketServer(ipAddress,port);
            NetworkGame.OnLineEnable += OnLineEnable;
            NetworkGame.ServerCreateComplete += OnServerCreateComplete;
            NetworkGame.ConnectToServerComplete += OnConnectToServerComplete;
            NetworkGame.ClientDisconnect += OnClientDisconnect;
            NetworkGame.StartNetwork();
        }

        private void OnClientDisconnect()
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected();
            }
        }

        private void OnConnectToServerComplete(string name, string ipAddress, string port)
        {
            if (ClientIsConnected != null)
            {
                ClientIsConnected(name, ipAddress, port);
            }
        }

        private void OnServerCreateComplete(string name, string ipAddress, string port)
        {
            if (ServerIsCreated != null)
            {
                ServerIsCreated(name, ipAddress, port);
            }
        }

        private void OnLineEnable(Vector2 pos, LogicLine.Positioning positioning, int nextTurn)
        {
            MultiplayerGameController.Instance.EnableLine(pos, positioning, nextTurn);
        }

        private void OnStartGame(int fieldSize)
        {
            if (StartGameEvent != null)
            {
                StartGameEvent(fieldSize);
            }
        }

        public void StartClient(string ipAddress, string port)
        {
            NetworkGame = new AsyncSocketClient(ipAddress, port);
            NetworkGame.OnStartGame += OnStartGame;
            NetworkGame.OnLineEnable += OnLineEnable;
            NetworkGame.ServerCreateComplete += OnServerCreateComplete;
            NetworkGame.ConnectToServerComplete += OnConnectToServerComplete;
            NetworkGame.StartNetwork();
        }

        public void StartGame(int fieldSize)
        {
            NetworkGame.SendStartGame(fieldSize);
        }
    }
}
