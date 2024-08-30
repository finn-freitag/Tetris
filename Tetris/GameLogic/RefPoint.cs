using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public class RefPoint
    {
        public int X = 0;
        public int Y = 0;

        public RefPoint() { }

        public RefPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public RefPoint Clone()
        {
            return new RefPoint(X, Y);
        }

        public static implicit operator RefPoint(Point point)
        {
            return new RefPoint(point.X, point.Y);
        }

        public static implicit operator Point(RefPoint rp)
        {
            return new Point(rp.X, rp.Y);
        }
    }
}
