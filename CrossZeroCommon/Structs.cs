using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossZeroCommon
{
    public struct Vector2
    {
        public readonly int X;
        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public override string ToString()
        {
            return "X = " + X + ", Y = " + Y;
        }
    }
}
