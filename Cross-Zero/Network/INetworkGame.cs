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

        event Action<string, string, string> ServerCreateComplete;
        event Action<string, string, string> ConnectToServerComplete;

        event Action<int> OnStartGame;
        event Action<Vector2, LogicLine.Positioning, int> OnLineEnable;
    }
}
