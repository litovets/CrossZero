using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero.Logic
{
    public class MultiplayerGameController : GameController
    {
        public static new MultiplayerGameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MultiplayerGameController();
                    return _instance;
                }
                return _instance;
            }
        }

        private MultiplayerGameController(){}

        private static MultiplayerGameController _instance;

        public override void StartGame(int fieldSize)
        {
            CreateGame(fieldSize);
        }
    }
}
