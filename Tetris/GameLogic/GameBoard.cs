using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public class GameBoard
    {
        private List<Tile> Tiles = new List<Tile>();

        public Tile MovingTile = TileRegistry.GetRandom();

        public Tile NextTile = TileRegistry.GetRandom();

        public Size Size = new Size(10, 20);

        public GameBoard(Size Size)
        {
            this.Size = Size;
            Tiles.Add(MovingTile);
            PlaceOnStart(MovingTile);
        }

        public (Tile Tile, RefPoint Point) GetPosAt(int x, int y)
        {
            for (int i = 0; i < Tiles.Count; i++)
                for (int j = 0; j < Tiles[i].Points.Count; j++)
                    if (Tiles[i].Points[j].X == x && Tiles[i].Points[j].Y == y)
                        return (Tiles[i], Tiles[i].Points[j]);
            return (null, null);
        }

        private void DeleteAt(int x, int y)
        {
            var pos = GetPosAt(x, y);
            if (pos.Tile == null)
                return;
            pos.Tile.Points.Remove(pos.Point);
            if (pos.Tile.Points.Count == 0)
                Tiles.Remove(pos.Tile);
        }

        private bool RemoveFullLines()
        {
            bool found = false;
            for(int y = 0; y < Size.Height; y++)
            {
                bool lineFull = true;
                for (int x = 0; x < Size.Width; x++)
                    if (GetPosAt(x, y).Tile == null)
                        lineFull = false;
                if (lineFull)
                {
                    found = true;
                    for (int x = 0; x < Size.Width; x++)
                        DeleteAt(x, y);
                    for (int y2 = y; y2 >= 0; y2--)
                        for (int x = 0; x < Size.Width; x++)
                        {
                            var r = GetPosAt(x, y2);
                            if (r.Tile != null)
                                r.Point.Y++;
                        }
                }
            }
            return found;
        }

        public void MoveVertical(Tile t, int distance)
        {
            if (distance == 0)
                return;
            if (t.Points[t.Points.Count - 1].Y + distance >= Size.Height)
                return;
            for (int i = 0; i < t.Points.Count; i++)
            {
                t.Points[i].Y += distance;
            }
        }

        public void MoveHorizontal(Tile t, int distance)
        {
            if (distance == 0)
                return;
            if (distance < 0 && !IsSpaceLeft(t))
                return;
            if (distance > 0 && !IsSpaceRight(t))
                return;
            if (t.Points[0].X + distance < 0 || t.Points[t.Points.Count - 1].X + distance >= Size.Width)
                return;
            for (int i = 0; i < t.Points.Count; i++)
            {
                t.Points[i].X += distance;
            }
        }

        private RefPoint GetCenter(Tile t)
        {
            int avgY = (int)t.Points.Average((rp) => { return rp.Y; });
            int avgX = (int)t.Points.Average((rp) => { return rp.X; });
            return new RefPoint(avgX, avgY);
        }

        public void Rotate(Tile t)
        {
            List<RefPoint> points = new List<RefPoint>();
            List<RefPoint> backup = t.Points;
            RefPoint rp = GetCenter(t);
            t.Points = new List<RefPoint>(t.Points.Select((r) => { return new RefPoint(r.X - rp.X, r.Y - rp.Y); }));
            for (int i = 0; i < t.Points.Count; i++)
            {
                points.Add(new RefPoint(t.Points[i].Y, t.Size.Width - 1 - t.Points[i].X));
            }
            t.Size = new Size(0, 0);
            t.Size.Width = points.Max((r) => { return r.X; });
            t.Size.Height = points.Max((r) => { return r.Y; });
            t.Points = new List<RefPoint>(points.Select((r) => { return new RefPoint(r.X + rp.X, r.Y + rp.Y); }));
            RefPoint newRP = GetCenter(t);
            MoveHorizontal(t, rp.X - newRP.X);
            MoveVertical(t, rp.Y - newRP.Y);
            if (!FixTile(t))
                t.Points = backup;
        }

        private bool FixTile(Tile t, int depth = 0)
        {
            if (depth == 10)
                return false;
            bool overlap = false;
            for(int i = 0; i < t.Points.Count; i++)
            {
                var pos = GetPosAt(t.Points[i].X, t.Points[i].Y);
                if (pos.Tile != null && !t.Points.Contains(pos.Point))
                    overlap = true;
            }
            if (overlap)
                return false;
            for (int i = 0; i < t.Points.Count; i++)
            {
                if (t.Points[i].X < 0)
                {
                    MoveHorizontal(t, 1);
                    return FixTile(t, depth + 1);
                }
                if (t.Points[i].X >= Size.Width)
                {
                    MoveHorizontal(t, -1);
                    return FixTile(t, depth + 1);
                }
            }
            return true;
        }

        public bool IsSpaceDown(Tile t)
        {
            bool space = true;
            for(int i = 0; i < t.Points.Count; i++)
            {
                if (t.Points[i].Y + 1 >= Size.Height)
                    return false;
                var down = GetPosAt(t.Points[i].X, t.Points[i].Y + 1);
                if (down.Tile != null && !t.Points.Contains(down.Point))
                    space = false;
            }
            return space;
        }

        private bool IsSpaceLeft(Tile t)
        {
            bool space = true;
            for(int i = 0; i < t.Points.Count; i++)
            {
                if (t.Points[i].X - 1 < 0)
                    return false;
                var left = GetPosAt(t.Points[i].X - 1, t.Points[i].Y);
                if (left.Tile != null && !t.Points.Contains(left.Point))
                    space = false;
            }
            return space;
        }

        private bool IsSpaceRight(Tile t)
        {
            bool space = true;
            for(int i = 0; i < t.Points.Count; i++)
            {
                if (t.Points[i].X + 1 >= Size.Width)
                    return false;
                var right = GetPosAt(t.Points[i].X + 1, t.Points[i].Y);
                if (right.Tile != null && !t.Points.Contains(right.Point))
                    space = false;
            }
            return space;
        }

        private void PlaceOnStart(Tile t)
        {
            RefPoint rp = t.Points[0];
            MoveVertical(t, -(rp.Y + 1));
            MoveHorizontal(t, (int)(Size.Width / 2.0 - t.Size.Width / 2.0 - rp.X));
        }

        public bool Update()
        {
            if (IsSpaceDown(MovingTile))
            {
                MoveVertical(MovingTile, 1);
            }
            else
            {
                RemoveFullLines();

                for (int i = 0; i < MovingTile.Points.Count; i++)
                    if (MovingTile.Points[i].Y < 0)
                        return false;

                MovingTile = NextTile;
                NextTile = TileRegistry.GetRandom();
                Tiles.Add(MovingTile);
                PlaceOnStart(MovingTile);
            }
            return true;
        }
    }
}
