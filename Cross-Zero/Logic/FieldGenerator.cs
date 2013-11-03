using CrossZeroCommon;

namespace Cross_Zero.Logic
{
    public class FieldGenerator
    {
        private const int sideWidth = 3;
        private int fieldSize;

        private static FieldGenerator _instance;
        private FieldGenerator(){}

        public static FieldGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FieldGenerator();
                    return _instance;
                }
                return _instance;
            }
        }

        public void GenerateField(LogicRectangle[][] gameField)
        {
            fieldSize = gameField.Length;
            int num = sideWidth;
            int index = 0;
            int counter = 0;
            bool back = false;
            //Generate arrays and Rectangles
            do
            {
                gameField[index] = new LogicRectangle[num];
                for (int i = 0; i < num; i++)
                {
                    gameField[index][i] =
                        new LogicRectangle(new LogicRectangle.LineState(), new LogicRectangle.LineState(), new LogicRectangle.LineState(), new LogicRectangle.LineState());
                    gameField[index][i].SetPosition(index, i);
                }
                if (num == fieldSize) ++counter;
                if (num < fieldSize && !back)
                    num += 2;

                if (counter == sideWidth) back = true;

                if (back)
                    num -= 2;
                ++index;
            } while (num >= sideWidth);
        }

        public void GenerateStates(LogicRectangle[][] gameField)
        {
            //Horizontal cycle
            int index = 0;
            LogicLine line;
            LogicRectangle rect;
            LogicRectangle prevRect;
            do
            {
                for (int i = 0; i < gameField[index].Length; i++)
                {
                    rect = gameField[index][i];
                    if (i == 0)
                    {
                        line = new LogicLine(UIController.Instance.GetNewLine());
                        line.SetupUIVLine(new Vector2(index, i), gameField[index].Length);
                        line.RectRight = rect;
                        rect.LineLeft.Line = line;
                        rect.LineLeft.Enabled = true;
                        line.EnableLine(true);
                        continue;
                    }
                    prevRect = gameField[index][i - 1];
                    line = new LogicLine(UIController.Instance.GetNewLine());
                    line.SetupUIVLine(new Vector2(index, i), gameField[index].Length);
                    line.RectLeft = prevRect;
                    line.RectRight = rect;
                    prevRect.LineRight.Line = line;
                    prevRect.LineRight.Enabled = false;
                    rect.LineLeft.Line = line;
                    rect.LineLeft.Enabled = false;
                    line.EnableLine(false);

                    if (i == gameField[index].Length - 1)
                    {
                        line = new LogicLine(UIController.Instance.GetNewLine());
                        line.SetupUIVLine(new Vector2(index, ++i), gameField[index].Length);
                        line.RectLeft = rect;
                        rect.LineRight.Line = line;
                        rect.LineRight.Enabled = true;
                        line.EnableLine(true);
                    }
                }
                ++index;
            } while (index < gameField.Length);

            //Vertical cycle
            index = 0;
            int min = (gameField.Length - sideWidth) / 2;
            int counter = 0;
            int xPos = 0;
            bool down = false;
            do
            {
                int j = xPos;
                int inCounter = 0;
                int max = min + gameField[index].Length - 1;
                bool back = false;
                bool inFreeze = false;
                for (int i = min; i <= max; i++)
                {
                    rect = gameField[i][j];
                    if (i == min)
                    {
                        line = new LogicLine(UIController.Instance.GetNewLine());
                        line.SetupUIHLine(new Vector2(i, index));
                        line.RectRight = rect;
                        rect.LineTop.Line = line;
                        rect.LineTop.Enabled = true;
                        line.EnableLine(true);
                    }
                    if (i > min && i <= max)
                    {
                        if (!inFreeze && !back) prevRect = gameField[i - 1][j - 1];
                        else if (inFreeze && !back) prevRect = gameField[i - 1][j];
                        else prevRect = gameField[i - 1][j + 1];
                        line = new LogicLine(UIController.Instance.GetNewLine());
                        line.SetupUIHLine(new Vector2(i, index));
                        line.RectLeft = prevRect;
                        line.RectRight = rect;
                        prevRect.LineBottom.Line = line;
                        prevRect.LineBottom.Enabled = false;
                        rect.LineTop.Line = line;
                        rect.LineTop.Enabled = false;
                        line.EnableLine(false);
                    }

                    if (i == max)
                    {
                        line = new LogicLine(UIController.Instance.GetNewLine());
                        line.SetupUIHLine(new Vector2(i + 1, index));
                        line.RectLeft = rect;
                        rect.LineBottom.Line = line;
                        rect.LineBottom.Enabled = true;
                        line.EnableLine(true);
                    }

                    if (j == index) { ++inCounter; inFreeze = true; }
                    if (j < index && !back) ++j;
                    if (inCounter == sideWidth) { back = true; inFreeze = false; }
                    if (back) --j;
                }
                if (min == 0) { ++counter; }
                if (min == 0 && counter < sideWidth) xPos++;
                if (min > 0 && !down) --min;
                if (counter == sideWidth) { down = true; }
                if (down) ++min;
                if (min > 0 && counter == sideWidth) xPos += 2;

                ++index;
            } while (index < gameField.Length);
        }

        public void GeneratePoints(LogicRectangle[][] gameField)
        {
            int x, y;
            for (int i = 0; i < gameField.Length; i++)
            {
                for (int j = 0; j < gameField[i].Length; j++)
                {
                    //Set top point
                    if (gameField[i][j].LineLeft.Line.UIPointTop == null)
                    {
                        x = (int) (gameField[i][j].LineLeft.Line.UiLine.X1);
                        y = (int) (gameField[i][j].LineLeft.Line.UiLine.Y1);
                        gameField[i][j].LineLeft.Line.UIPointTop = UIController.Instance.GetNewPoint(new Vector2(x, y));
                    }
                    //Set bottom point
                    x = (int) (gameField[i][j].LineLeft.Line.UiLine.X2);
                    y = (int) (gameField[i][j].LineLeft.Line.UiLine.Y2);
                    gameField[i][j].LineLeft.Line.UIPointBottom =
                        UIController.Instance.GetNewPoint(new Vector2(x, y));

                    if (j == gameField[i].Length - 1)
                    {
                        //Set top point
                        if (gameField[i][j].LineRight.Line.UIPointTop == null)
                        {
                            x = (int)(gameField[i][j].LineRight.Line.UiLine.X1);
                            y = (int)(gameField[i][j].LineRight.Line.UiLine.Y1);
                            gameField[i][j].LineRight.Line.UIPointTop = UIController.Instance.GetNewPoint(new Vector2(x, y));
                        }
                        //Set bottom point
                        x = (int)(gameField[i][j].LineRight.Line.UiLine.X2);
                        y = (int)(gameField[i][j].LineRight.Line.UiLine.Y2);
                        gameField[i][j].LineRight.Line.UIPointBottom = UIController.Instance.GetNewPoint(new Vector2(x, y));
                    }
                }
            }
        }
    }
}
