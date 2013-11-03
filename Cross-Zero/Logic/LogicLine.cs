using System;
using System.Windows.Input;
using System.Windows.Shapes;
using CrossZeroCommon;

namespace Cross_Zero.Logic
{
    public class LineEventArgs : EventArgs
    {
        public bool flag;
    }

    public class LogicLine
    {
        public LogicRectangle RectLeft { get; set; }
        public LogicRectangle RectRight { get; set; }
        public Ellipse UIPointTop { get; set; }
        public Ellipse UIPointBottom { get; set; }
        public Line UiLine { get { return _uiLine; }}

        private readonly Line _uiLine;
        private Vector2 _pos;

        public event EventHandler<LineEventArgs> LineEnable;

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
            UIController.Instance.SetupVLine(_uiLine, pos, rowLength);
        }

        public void SetupUIHLine(Vector2 pos)
        {
            _pos = pos;
            UIController.Instance.SetupHLine(_uiLine, pos);
        }

        ~LogicLine()
        {
            _uiLine.MouseLeftButtonUp -= UiLineOnMouseLeftButtonUp;
        }
    }
}
