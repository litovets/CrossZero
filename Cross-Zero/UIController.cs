using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CrossZeroCommon;
using Cross_Zero.Logic;
using Cross_Zero.Network;

public delegate void NetworkEvent();

namespace Cross_Zero
{
    public class UIController
    {
        public const int LineThickness = 4;
        public const int PointRadius = LineThickness + 1;
        public const int CellWidth = 24;
        public SolidColorBrush LineEnterColor = Brushes.Black;
        public SolidColorBrush LineLeaveColor = Brushes.Transparent;
        public SolidColorBrush P1LineEnabledColor = Brushes.Black;
        public SolidColorBrush P2LineEnabledColor = Brushes.Red;

        public event Action UiOperationComplete;

        private object savedData;

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
        }

        public void StartListenNetworkEvents()
        {
            NetworkManager.Instance.ServerIsCreated += OnServerIsCreated;
            NetworkManager.Instance.ClientIsConnected += OnServerIsCreated;
            NetworkManager.Instance.StartGameEvent += OnStartGameEvent;
            StartListenNextTurnEvent();
        }

        public void StartListenNextTurnEvent()
        {
            GameController.Instance.NextPlayerEvent += OnNextPlayerEventLocal;
        }

        private void OnStartGameEvent(int fieldSize)
        {
            savedData = fieldSize;
            ConnectedListBox.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NetworkEvent(ShowGameCanvas));
        }

        private void ShowGameCanvas()
        {
            ConnectionListCanvas.Visibility = Visibility.Hidden;
            Canvas.Visibility = Visibility.Visible;
            int fieldSize = (int) savedData;
            MultiplayerGameController.Instance.StartGame(fieldSize);
        }

        private void OnServerIsCreated(string name, string ipAddress, string port)
        {
            string[] data = {name, ipAddress, port};
            savedData = data;

            //List<string> list = new List<string> {name, ipAddress, port};
            ConnectedListBox.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NetworkEvent(CreateNewListBoxItem));
            
        }

        private void CreateNewListBoxItem()
        {
            ListBoxItem lbi = new ListBoxItem();
            string[] data = (string[]) savedData;
            lbi.Content = string.Format("{0}\t{1}\t{2}", data[0], data[1], data[2]);
            ConnectedListBox.Items.Add(lbi);
            if (NetworkManager.Instance.IsServer && ConnectedListBox.Items.Count == 2)
                StartGameButton.Visibility = Visibility.Visible;

        }

        private void OnNextPlayerEventLocal()
        {
            ActivePlayerLabel.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NetworkEvent(SetNextPlayerName));
        }

        private void SetNextPlayerName()
        {
            ActivePlayerLabel.Content = "Ход " + GameController.Instance.Players[GameController.Instance.ActivePlayerId].Name;
        }
        
        private static UIController _instance;
        
        public Canvas Canvas { get; set; }
        public Label ActivePlayerLabel { get; set; }
        public Label LinePos { get; set; }
        public ListBox ConnectedListBox { get; set; }
        public Button StartGameButton { get; set; }
        public Canvas ConnectionListCanvas { get; set; }

        private LogicLine lineForEnable;

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
            point.Fill = P1LineEnabledColor;
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
            if (GameController.Instance.ActivePlayerId == 0)
                label.Foreground = P1LineEnabledColor;
            else
                label.Foreground = P2LineEnabledColor;

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
            

            if (GameController.Instance.ActivePlayerId == 0)
                line.Stroke = P1LineEnabledColor;
            else
                line.Stroke = P2LineEnabledColor;

            ShowEnableLineAnimation(line);

            line.MouseEnter -= LineOnMouseEnter;
            line.MouseLeave -= LineOnMouseLeave;
        }

        private void ShowEnableLineAnimation(Line line)
        {
            double normalStrokeThickness = line.StrokeThickness;
            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            //anim.RepeatBehavior = new RepeatBehavior(2);
            anim.AutoReverse = true;
            anim.From = normalStrokeThickness;
            anim.To = normalStrokeThickness + 2.0;
            line.BeginAnimation(Line.StrokeThicknessProperty, anim);
            ColorAnimation c_anim = new ColorAnimation();
            c_anim.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            //c_anim.RepeatBehavior = new RepeatBehavior(2);
            c_anim.AutoReverse = true;
            c_anim.To = Colors.Transparent;
            SolidColorBrush brush = new SolidColorBrush(((SolidColorBrush)line.Stroke).Color);
            line.Stroke = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, c_anim);
        }

        public void NetRequestEnableLine(LogicLine line)
        {
            lineForEnable = line;
            line.UiLine.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NetworkEvent(EnableLine));
        }

        private void EnableLine()
        {
            lineForEnable.EnableLine(true);
            if (UiOperationComplete != null)
                UiOperationComplete();
        }

        #endregion
    }
}
