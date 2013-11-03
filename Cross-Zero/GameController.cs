using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross_Zero
{
    public class GameController
    {
        public const int minFieldSize = 5;
        public const int maxFieldSize = 21;

        public int FieldSize { get; set; }
        public int CellWidth { get; set; }

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
        private GameController(){}

        private static GameController _instance;

        private LogicRectangle[][] _gameField;

        public void StartGame(int fieldSize)
        {
            FieldSize = fieldSize;
            _gameField = new LogicRectangle[FieldSize][];
            UIController.Instance.CellWidth = CellWidth;
            FieldGenerator.Instance.GenerateField(_gameField);
            FieldGenerator.Instance.GenerateStates(_gameField);
        }
    }
}
