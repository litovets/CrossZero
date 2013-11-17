using System;
using System.Windows.Input;
using System.Windows.Shapes;
using CrossZeroCommon;
using Cross_Zero.Network;

namespace Cross_Zero.Logic
{
    public class LineEventArgs : EventArgs
    {
        public bool flag;
        public Vector2 pos;
        public LogicLine.Positioning positioning;
    }

    public class LogicLine
    {
        public enum Positioning
        {
            Horizontal,
            Vertical
        }

        public LogicRectangle RectLeft { get; set; }
        public LogicRectangle RectRight { get; set; }
        public Ellipse UIPointTop { get; set; }
        public Ellipse UIPointBottom { get; set; }
        public Line UiLine { get { return _uiLine; }}
        public Vector2 Pos {get { return _pos; }}

        private readonly Line _uiLine;
        private Vector2 _pos;

        public event EventHandler<LineEventArgs> LineEnabled;

        private Positioning _positioning;

        public LogicLine(Line line)
        {
            _uiLine = line;
            if (_uiLine != null)
            {
                _uiLine.MouseLeftButtonUp += UiLineOnMouseLeftButtonUp;
            }
        }

        private void UiLineOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            //Send event
            LineEventArgs lineEventArgs = new LineEventArgs { pos = _pos, positioning = _positioning };
            LineEnabled(this, lineEventArgs);

            UIController.Instance.LinePos.Content = _pos;

            EnableLine(true);
        }

        public void EnableLine(bool flag)
        {
            if (RectLeft != null)
            {
                RectLeft.EnableLine(this, flag);
            }
            if (RectRight != null)
            {
                RectRight.EnableLine(this, flag);
            }

            if (flag)
            {
                UIController.Instance.EnableLine(_uiLine);
                _uiLine.MouseLeftButtonUp -= UiLineOnMouseLeftButtonUp;
            }
        }

        public void SetupUIVLine(Vector2 pos, int rowLength)
        {
            _pos = pos;
            _positioning = Positioning.Vertical;
            UIController.Instance.SetupVLine(_uiLine, pos, rowLength);

            GameController.Instance.LogicLines.Add(this);
        }

        public void SetupUIHLine(Vector2 pos)
        {
            _pos = pos;
            _positioning = Positioning.Horizontal;
            UIController.Instance.SetupHLine(_uiLine, pos);

            GameController.Instance.LogicLines.Add(this);
        }

        ~LogicLine()
        {
            _uiLine.MouseLeftButtonUp -= UiLineOnMouseLeftButtonUp;
        }
    }
}
