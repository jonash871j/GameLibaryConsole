namespace Engine
{
    public class TileMap
    {
        public int[,] IdMap { get; private set; } = null;
        public Sprite[] Sprites { get; private set; } = null;

        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public int TileWidth { get; private set; } = 0;
        public int TileHeight { get; private set; } = 0;

        public TileMap(int[,] idMap, int tileWidth, int tileHeight)
            : this(idMap, new Sprite[256], tileWidth, tileHeight) 
        {
            for (int i = 0; i < Sprites.Length; i++)
                Sprites[i] = null;
        }

        public TileMap(int[,] idMap, Sprite[] sprites, int tileWidth, int tileHeight)
        {
            IdMap = idMap;
            Sprites = sprites;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            Width = idMap.GetLength(1);
            Height = idMap.GetLength(0);
        }
    }
}
