using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CrossZeroCommon;

namespace Cross_Zero
{
    public class UIController
    {
        public const int LineThickness = 4;
        public SolidColorBrush LineEnterColor = Brushes.Magenta;
        public SolidColorBrush LineLeaveColor = Brushes.LightSkyBlue;
        public SolidColorBrush LineEnabledColor = Brushes.Black;

        public static UIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIController();
                    return _instance;
                }
                return _instance;
            }
        }

        public int CellWidth { get; set; }

        private UIController(){}

        private static UIController _instance;
        public Canvas Canvas { get; set; }

        public Line GetNewHLine()
        {
            Line line = new Line();
            line.StrokeThickness = LineThickness;
            line.Stroke = LineLeaveColor;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = CellWidth;
            line.Y2 = 0;
            line.MouseEnter += LineOnMouseEnter;
            line.MouseLeave += LineOnMouseLeave;
            Canvas.Children.Add(line);
            return line;
        }

        public Line GetNewVLine()
        {
            Line line = new Line();
            line.StrokeThickness = LineThickness;
            line.Stroke = LineLeaveColor;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.MouseEnter += LineOnMouseEnter;
            line.MouseLeave += LineOnMouseLeave;
            Canvas.Children.Add(line);
            //Canvas.Visibility = Visibility.Hidden;
            //line.Visibility = Visibility.Hidden;
            return line;
        }

        public void SetupHLine(Line line, Vector2 pos)
        {
            int fieldSize = GameController.Instance.FieldSize;
            int halfCellWidth = (int) (CellWidth*0.5f);
            int centerIndex = (int) ((fieldSize - 1)*0.5f);
            Vector2 centerPos = new Vector2((int) (Canvas.Width*0.5d), (int)(Canvas.Height*0.5d));
            int y = halfCellWidth + CellWidth*(centerIndex - pos.X);
            int x = (pos.Y - centerIndex)*CellWidth;
            line.X1 = centerPos.X + x - halfCellWidth;
            line.Y1 = centerPos.Y + y;
            line.X2 = centerPos.X + x + halfCellWidth;
            line.Y2 = centerPos.Y + y;
        }

        public void SetupVLine(Line line, Vector2 pos, int rowLength)
        {
            int fieldSize = GameController.Instance.FieldSize;
            int halfCellWidth = (int)(CellWidth * 0.5f);
            int centerIndex = (int)((fieldSize - 1) * 0.5f);
            Vector2 centerPos = new Vector2((int)(Canvas.Width * 0.5d), (int)(Canvas.Height * 0.5d));
            int x = -halfCellWidth - CellWidth*(centerIndex - pos.Y - (int) ((fieldSize - rowLength)*0.5f));
            int y = (centerIndex - pos.X)*CellWidth;
            line.X1 = centerPos.X + x;
            line.Y1 = centerPos.Y + y - halfCellWidth;
            line.X2 = centerPos.X + x;
            line.Y2 = centerPos.Y + y + halfCellWidth;
        }

        #region Event Handlers

        private void LineOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            Line line = (Line) sender;
            line.Stroke = LineEnterColor;
        }

        private void LineOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            Line line = (Line)sender;
            line.Stroke = LineLeaveColor;
        }

        public void EnableLine(Line line)
        {
            line.Stroke = LineEnabledColor;
            line.MouseEnter -= LineOnMouseEnter;
            line.MouseLeave -= LineOnMouseLeave;
        }

        #endregion
    }
}
