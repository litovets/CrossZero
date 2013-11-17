using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossZeroCommon;
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

        protected MultiplayerGameController(){}

        private static MultiplayerGameController _instance;

        public override void StartGame(int fieldSize)
        {
            NetworkManager.Instance.IsMultiplayerGame = true;

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

        private void LogicLineOnEnabled(object sender, LineEventArgs args)
        {
            NetworkManager.Instance.NetworkGame.SendEnableLine(args.pos, args.positioning);
        }

        public void EnableLine(Vector2 pos, LogicLine.Positioning positioning)
        {
            int x = pos.X == 0 ? pos.X : pos.X - 1;
            int y = pos.Y == 0 ? pos.Y : pos.Y - 1;
            LogicRectangle rect = _gameField[x][y];
            LogicLine line = rect.GetLine(pos, positioning);
            line.EnableLine(true);
        }
    }
}
