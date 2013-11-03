using CrossZeroCommon;

namespace Cross_Zero.Logic
{
    public class LogicRectangle
    {
        public Vector2 Pos { get; set; }

        public class LineState
        {
            public LogicLine Line { get; set; }
            public bool Enabled { get; set; }

            public LineState()
            {
                Enabled = false;
            }
        }

        public LineState LineTop { get; set; }
        public LineState LineBottom { get; set; }
        public LineState LineLeft { get; set; }
        public LineState LineRight { get; set; }

        public bool IsComplete
        {
            get { return LineTop.Enabled && LineBottom.Enabled && LineLeft.Enabled && LineRight.Enabled; }
        }

        public LogicRectangle(LineState top, LineState bot, LineState left, LineState right)
        {
            LineTop = top;
            LineBottom = bot;
            LineLeft = left;
            LineRight = right;
        }

        public void SetPosition(int row, int column)
        {
            Pos = new Vector2(row, column);
        }

        public void EnableLine(LogicLine line, bool flag)
        {
            if (LineTop.Line == line)
                LineTop.Enabled = flag;

            if (LineBottom.Line == line)
                LineBottom.Enabled = flag;

            if (LineLeft.Line == line)
                LineLeft.Enabled = flag;

            if (LineRight.Line == line)
                LineRight.Enabled = flag;

            //if (IsComplete) Debug.Log("COMPLETE, POS = " + Pos);
        }
    }
}
