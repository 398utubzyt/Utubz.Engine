using Utubz.Graphics;

namespace Utubz.Flat
{
    public sealed class Tilemap : Object
    {
        private class Map : Object
        {
            public int Width;
            public int[] Tiles;

            public int GetFromPosition(int x, int y)
                => Tiles[x + (y * Width)];

            public int GetFromIndex(int i)
                => Tiles[i];
        }

        private Map map;
        public Spritesheet Sheet { get; set; }
        public int Width { get => map.Width; set => map.Width = value; }
        public int Height { get => map.Tiles.Length / map.Width; set => System.Array.Resize(ref map.Tiles, value * map.Width); }
        public Sprite GetTile(int x, int y) => Sheet.Get(map.GetFromPosition(x, y));

        protected override void Clean()
        {
            map.Destroy();
        }

        public Tilemap(Spritesheet sheet, int width, int height)
        {
            map = new Map();
            map.Width = width;
            map.Tiles = new int[width * height];

            Sheet = sheet;
        }
    }
}
