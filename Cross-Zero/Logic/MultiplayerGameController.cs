using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            CreateGame(fieldSize);

            foreach (LogicLine logicLine in LogicLines)
            {
                logicLine.LineEnabled += LogicLineOnEnabled;
            }

            for (int i = 0; i < _gameField.Length; i++)
            {
                RectsCount += _gameField[i].Length;
                for (int j = 0; j < _gameField[i].Length; j++)
                {
                    _gameField[i][j].RectCompleted += OnRectCompleted;
                }
            }

            ActivePlayerId = 0;

            UIController.Instance.ActivePlayerLabel.Content = Players[ActivePlayerId].Name;

            OnEndCreateGame();
            IsGameStarted = true;
        }

        public void CreatePlayer(int id, string name, string sign, bool setCurrent)
        {
            if (Players == null)
                Players = new Player[2];
            Players[id] = new Player(id, name, sign);
            if (setCurrent) CurrentPlayer = Players[id];
        }

        private void LogicLineOnEnabled(object sender, LineEventArgs args)
        {
            NextPlayer();
            OnNextPlayer();
            int nextTurnValue = TurnAgain ? 0 : 1;
            NetworkManager.Instance.NetworkGame.SendEnableLine(args.pos, args.positioning, nextTurnValue);
            TurnAgain = false;
        }


        public void EnableLine(Vector2 pos, LogicLine.Positioning positioning, int nextTurn)
        {
            /*int x = pos.X == 0 ? pos.X : pos.X - 1;
            int y = pos.Y == 0 ? pos.Y : pos.Y - 1;
            LogicRectangle rect = _gameField[x][y];
            LogicLine line = rect.GetLine(pos, positioning);*/
            var result = from logicLine in LogicLines
                where logicLine.Pos == pos && logicLine.LinePositioning == positioning
                select logicLine;
            LogicLine line = result.First();
            

            UIController.Instance.NetRequestEnableLine(line);
            if (nextTurn == 1)
            {
                TurnAgain = false;
                NextPlayer();
                OnNextPlayer();
            }
        }

    }
}
