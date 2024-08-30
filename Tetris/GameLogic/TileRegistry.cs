using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.GameLogic
{
    public static class TileRegistry
    {
        private static Random r = new Random();

        /// <summary>
        /// Parts must be rectangular, "x" is a box, " " is space, "|" is line break
        /// </summary>
        public static List<Tile> Tiles = new List<Tile>()
        {
            new Tile("x|x|x|x", Color.FromArgb(255, 0  , 0  )),
            new Tile(" xx|xx ", Color.FromArgb(0  , 255, 0  )),
            new Tile("xx | xx", Color.FromArgb(0  , 0  , 255)),
            new Tile("xxx| x ", Color.FromArgb(255, 255, 0  )),
            new Tile("xx|xx"  , Color.FromArgb(0  , 255, 255)),
            new Tile("x  |xxx", Color.FromArgb(255, 0  , 255)),
            new Tile("  x|xxx", Color.FromArgb(255, 255, 255)),
        };

        public static Tile GetRandom()
        {
            return Tiles[(int)(r.NextDouble() * Tiles.Count)].Clone();
        }
    }
}
