using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CrossZeroCommon;
using Cross_Zero.Logic;
using Cross_Zero.Network;

namespace Cross_Zero
{
    public class UIController
    {
        public const int LineThickness = 4;
        public const int PointRadius = LineThickness + 1;
        public const int CellWidth = 24;
        public SolidColorBrush LineEnterColor = Brushes.Black;
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

        private UIController()
        {
            signs = new List<Label>();

            GameController.Instance.NextPlayerEvent += OnNextPlayerEventLocal;
        }

        public void StartListenNetworkEvents()
        {
            NetworkManager.Instance.ServerIsCreated += OnServerIsCreated;
        }

        private void OnServerIsCreated(string name, string ipAddress, string port)
        {
            //List<string> list = new List<string> {name, ipAddress, port};
            ListBoxItem lbi = new ListBoxItem();
            lbi.Content = string.Format("{0}\t{1}\t{2}", name, ipAddress, port);
            ConnectedListBox.Items.Add(lbi);
        }

        private List<Label> signs;

        private void OnNextPlayerEventNetwork()
        {
            ActivePlayerLabel.Content = MultiplayerGameController.Instance.players[GameController.Instance.ActivePlayerId].Name;
        }

        private void OnNextPlayerEventLocal()
        {
            ActivePlayerLabel.Content = GameController.Instance.players[GameController.Instance.ActivePlayerId].Name;
        }

        private static UIController _instance;
        
        public Canvas Canvas { get; set; }
        public Label ActivePlayerLabel { get; set; }
        public Label LinePos { get; set; }
        public ListBox ConnectedListBox { get; set; }

        public Line GetNewLine()
        {
            Line line = new Line();
            line.StrokeThickness = LineThickness;
            line.Stroke = LineLeaveColor;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.MouseEnter += LineOnMouseEnter;
            line.MouseLeave += LineOnMouseLeave;
            Canvas.Children.Add(line);
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
            line.Y1 = centerPos.Y - y;
            line.X2 = centerPos.X + x + halfCellWidth;
            line.Y2 = centerPos.Y - y;
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
            line.Y1 = centerPos.Y - y - halfCellWidth;
            line.X2 = centerPos.X + x;
            line.Y2 = centerPos.Y - y + halfCellWidth;
        }

        public Ellipse GetNewPoint(Vector2 pos)
        {
            Ellipse point = new Ellipse {Width = PointRadius, Height = PointRadius};
            point.Fill = LineEnabledColor;
            Canvas.SetLeft(point, pos.X - PointRadius*0.5f);
            Canvas.SetTop(point, pos.Y - PointRadius*0.5f);
            Canvas.Children.Add(point);
            return point;
        }

        public void ActivateSigh(string sign, LogicRectangle rect)
        {
            Label label = new Label();
            label.Content = sign;
            label.FontSize = 20;
            label.FontWeight = FontWeights.Bold;
            label.Padding = new Thickness(0,0,0,0);

            double x = rect.LineLeft.Line.UiLine.X1+4;
            double y = rect.LineLeft.Line.UiLine.Y1-3;
            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, y);
            Canvas.Children.Add(label);
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
