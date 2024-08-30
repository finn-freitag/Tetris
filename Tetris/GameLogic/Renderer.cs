using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public class Renderer
    {
        public Size ImageSize = new Size(400, 700);
        public int TileSize = 20;
        public GameBoard GameBoard = null;
        public bool ShowNextTile = true;

        public Renderer()
        {

        }

        public Bitmap Render()
        {
            Size boardSizePx = new Size(TileSize * GameBoard.Size.Width, TileSize * GameBoard.Size.Height);
            if (boardSizePx.Width > ImageSize.Width || boardSizePx.Height > ImageSize.Height)
                throw new Exception("Board doesn't fit into image!");
            Size offset = new Size(ImageSize.Width / 2 - boardSizePx.Width / 2, ImageSize.Height / 2 - boardSizePx.Height / 2);
            Bitmap bmp = new Bitmap(ImageSize.Width, ImageSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.DrawRectangle(Pens.Gray, offset.Width, offset.Height, boardSizePx.Width, boardSizePx.Height);
            for(int y = 0; y < GameBoard.Size.Height; y++)
            {
                for(int x = 0; x < GameBoard.Size.Width; x++)
                {
                    var pos = GameBoard.GetPosAt(x, y);
                    if (pos.Tile == null)
                        continue;
                    g.FillRectangle(new SolidBrush(pos.Tile.Color), new Rectangle(pos.Point.X * TileSize + offset.Width, pos.Point.Y * TileSize + offset.Height, TileSize, TileSize));
                }
            }
            if (ShowNextTile)
                for (int i = 0; i < GameBoard.NextTile.Points.Count; i++)
                    g.FillRectangle(new SolidBrush(GameBoard.NextTile.Color), offset.Width + boardSizePx.Width + (2 + GameBoard.NextTile.Points[i].X) * TileSize, offset.Height + (1 + GameBoard.NextTile.Points[i].Y) * TileSize, TileSize, TileSize);
            g.Flush();
            g.Dispose();
            return bmp;
        }
    }
}
