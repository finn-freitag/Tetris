using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public class Tile
    {
        public List<RefPoint> Points = new List<RefPoint>();
        public Color Color = Color.Red;
        public Size Size = new Size();

        private Tile() { }

        public Tile(string tile, Color color)
        {
            Color = color;
            string[] lines = tile.Split('|');
            for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[y].Length; x++)
                    if (lines[y][x] == 'x')
                        Points.Add(new RefPoint(x, y));
            RefPoint last = Points[Points.Count - 1];
            Size = new Size(last.X, last.Y);
        }

        public Tile Clone()
        {
            Tile t = new Tile();
            for (int i = 0; i < Points.Count; i++)
                t.Points.Add(Points[i].Clone());
            t.Size = Size;
            t.Color = Color;
            return t;
        }
    }
}
