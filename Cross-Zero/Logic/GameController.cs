
using System;

namespace Cross_Zero.Logic
{
    public class GameController
    {
        public const int minFieldSize = 5;
        public const int maxFieldSize = 21;

        public int FieldSize { get; set; }

        public Player ServerPlayer { get; private set; }
        public Player RemotePlayer { get; private set; }
        public Player ActivePlayer { get; private set; }

        public event Action EndCreateGame;

        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameController();
                    return _instance;
                }
                return _instance;
            }
        }
        protected GameController(){}

        private static GameController _instance;

        protected LogicRectangle[][] _gameField;

        public virtual void StartGame(int fieldSize)
        {
            CreateGame(fieldSize);

            if (EndCreateGame != null)
                EndCreateGame();
        }

        public void CreateGame(int fieldSize)
        {
            FieldSize = fieldSize;
            _gameField = new LogicRectangle[FieldSize][];
            FieldGenerator.Instance.GenerateField(_gameField);
            FieldGenerator.Instance.GenerateStates(_gameField);
            FieldGenerator.Instance.GeneratePoints(_gameField);
        }
    }
}
