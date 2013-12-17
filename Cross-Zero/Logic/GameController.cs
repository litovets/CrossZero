
using System;
using System.Collections.Generic;
using System.Windows;
using Cross_Zero.Network;

namespace Cross_Zero.Logic
{
    public class GameController
    {
        public const int minFieldSize = 5;
        public const int maxFieldSize = 21;

        public int FieldSize { get; set; }

        public Player[] Players { get; protected set; }
        public Player CurrentPlayer { get; protected set; }
        public int ActivePlayerId { get; protected set; }

        public List<LogicLine> LogicLines { get; set; }
        public int RectsCount { get; protected set; }

        public event Action NextPlayerEvent;
        public event Action EndCreateGame;

        public bool IsGameStarted { get; protected set; }

        public static GameController Instance
        {
            get
            {
                if (NetworkManager.Instance.IsMultiplayerGame)
                {
                    /*if (_instance == null)
                    {
                        _instance = MultiplayerGameController.Instance;
                        return _instance;
                    }*/
                    return MultiplayerGameController.Instance;
                }

                if (_instance == null)
                {
                    _instance = new GameController();
                    return _instance;
                }
                return _instance;
            }
        }
        protected GameController(){ LogicLines = new List<LogicLine>(); }

        private static GameController _instance;

        protected bool TurnAgain { get; set; }

        protected LogicRectangle[][] _gameField;

        public virtual void StartGame(int fieldSize)
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

            Players = new[] {new Player(0, "Player1", "X"), new Player(1, "Player2", "O")};
            ActivePlayerId = 0;

            UIController.Instance.ActivePlayerLabel.Content = Players[ActivePlayerId].Name + " turn";

            OnEndCreateGame();
            IsGameStarted = true;
        }

        protected void OnRectCompleted(LogicRectangle logicRectangle)
        {
            Players[ActivePlayerId].AddRect();
            UIController.Instance.ActivateSigh(Players[ActivePlayerId].Sign, logicRectangle);
            if ((Players[0].ActivatedRects + Players[1].ActivatedRects) == RectsCount)
            {
                if (Players[0].ActivatedRects > Players[1].ActivatedRects)
                {
                    MessageBox.Show(
                        string.Format("{0} won with {1} rects.", Players[0].Name, Players[0].ActivatedRects),
                        "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Players[0].ActivatedRects < Players[1].ActivatedRects)
                {
                    MessageBox.Show(
                        string.Format("{0} won with {1} rects.", Players[1].Name, Players[1].ActivatedRects),
                        "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (Players[0].ActivatedRects == Players[1].ActivatedRects)
                {
                    MessageBox.Show("Draw in this game", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            TurnAgain = true;
        }

        private void LogicLineOnEnabled(object sender, LineEventArgs e)
        {
            NextPlayer();
            OnNextPlayer();
            TurnAgain = false;
        }

        public void CreateGame(int fieldSize)
        {
            FieldSize = fieldSize;
            _gameField = new LogicRectangle[FieldSize][];
            FieldGenerator.Instance.GenerateField(_gameField);
            FieldGenerator.Instance.GenerateStates(_gameField);
            FieldGenerator.Instance.GeneratePoints(_gameField);
        }

        public void NextPlayer()
        {
            if (TurnAgain)
            {
                return;
            }

            if (ActivePlayerId == 1)
                ActivePlayerId = 0;
            else
                ActivePlayerId++;
        }

        #region Event handlers

        public void OnEndCreateGame()
        {
            Action action = EndCreateGame;
            if (action != null)
            {
                action();
            }
        }

        public void OnNextPlayer()
        {
            Action action = NextPlayerEvent;
            if (action != null)
            {
                action();
            }
        }

        #endregion
    }
}
