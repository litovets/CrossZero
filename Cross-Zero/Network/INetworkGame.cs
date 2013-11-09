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
        void StartGame(int fieldSize);
        void EnableLine(bool flag, Vector2 pos, LogicLine.Positioning positioning);

    }
}
