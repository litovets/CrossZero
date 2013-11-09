using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cross_Zero.Network;

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

            foreach (LogicLine logicLine in LogicLines)
            {
                logicLine.LineEnabled += LogicLineOnEnabled;
            }

            for (int i = 0; i < _gameField.Length; i++)
            {
                for (int j = 0; j < _gameField[i].Length; j++)
                {
                    _gameField[i][j].RectCompleted += OnRectCompleted;
                }
            }


        }

        private void OnRectCompleted(LogicRectangle obj)
        {
        }

        private void LogicLineOnEnabled(object sender, LineEventArgs args)
        {
            NetworkManager.Instance.NetworkGame.EnableLine(args.flag, args.pos, args.positioning);
        }
    }
}
