using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void StartServer()
        {
            AsyncSocketServer.StartListening();
            
            NetworkGame = new AsyncSocketServer();
        }

        public void StartClient()
        {
            AsyncSocketClient.StartClient();

            NetworkGame = new AsyncSocketClient();
        }
    }
}
