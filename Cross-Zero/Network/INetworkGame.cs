using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossZeroCommon;
using Cross_Zero.Logic;

namespace Cross_Zero.Network
{
    public interface INetworkGame
    {
        void StartNetwork();

        void SendUsername(string username);

        void SendStartGame(int fieldSize);

        void SendEnableLine(Vector2 pos, LogicLine.Positioning positioning, int nextTurn);

        Action<string, string, string> ServerCreateComplete { get; set; }
        Action<string, string, string> ConnectToServerComplete { get; set; }
        Action ClientDisconnect { get; set; }

        Action<int> OnStartGame { get; set; }
        Action<Vector2, LogicLine.Positioning, int> OnLineEnable { get; set; }
    }
}
