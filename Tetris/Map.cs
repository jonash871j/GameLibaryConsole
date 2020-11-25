namespace TetrisLogic
{
    public class Map
    {
        private Field[,] array;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Field this[int x, int y]
        {
            get 
            {
                if (CheckInbound(x, y))
                    return array[x, y];
                else
                    return Field.Out;
            }
            set
            {
                if (CheckInbound(x, y))
                    array[x, y] = value;
            }
        }

        public Map(int width = 10, int height = 21)
        {
            Width = width;
            Height = height;
            array = new Field[width, height];
            Reset();
        }

        public void Reset()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = Field.Empty;
                }
            }
        }

        public bool CheckTetrominoCollision(Tetromino tetromino)
        {
            // Checks collision with another tetromino
            for (int y = 0; y < tetromino.Height; y++)
            {
                for (int x = 0; x < tetromino.Width; x++)
                {
                    // Skips empty fields
                    if (tetromino[x, y] != Field.Empty)
                    {
                        // Checks for solid field
                        if (CheckSolidField(tetromino.X + x, tetromino.Y + y))
                            return true;

                        // Checks if tetromino reached the bottom
                        if (tetromino.Y + y >= Height)
                            return true;
                    }
                }
            }

            return false;
        }

        public void PlaceTetromino(Tetromino tetromino)
        {
            for (int y = 0; y < tetromino.Height; y++)
            {
                for (int x = 0; x < tetromino.Width; x++)
                {
                    if (tetromino[x, y] != Field.Empty)
                        this[x + tetromino.X, y + tetromino.Y] = tetromino[x, y];
                }
            }

            tetromino.Reset(Width / 2, -4);
        }

        public bool CheckInbound(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= Width) || (y >= Height))
                return false;
            else
                return true;
        }

        public bool CheckSolidField(int x, int y)
        {
            if ((this[x, y] == Field.Empty) || (this[x, y] == Field.Out))
                return false;
            else
                return true;
        }
    }
}
