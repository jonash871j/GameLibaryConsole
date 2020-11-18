namespace TicTacToe
{
    public class Map
    {
        public int Size { get; }
        public Brick[,] MapArray { get; set; }

        public Map(int size)
        {
            Size = size;
            MapArray = new Brick[Size, Size];

            Reset();
        }

        public void Reset()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    MapArray[y, x] = new Brick(Brick.Type.None);
                }
            }
        }

        private bool CheckWinForBrick(Brick.Type brickType)
        {
            for (int y = 0; y < Size; y++)
            {
                int xCount = 0, yCount = 0;
                int xYCount = 0, yXCount = 0;

                for (int x = 0; x < Size; x++)
                {
                    if (MapArray[x, y].BrickType == brickType) 
                        xCount++;
                    if (MapArray[y, x].BrickType == brickType) 
                        yCount++;
                    if (MapArray[x, x].BrickType == brickType) 
                        xYCount++;
                    if (MapArray[x, Size - x - 1].BrickType == brickType) 
                        yXCount++;
                }

                if ((xCount == Size) || (yCount == Size) || (xYCount == Size) || (yXCount == Size))
                    return true;
            }
            return false;
        }
        private bool CheckDraw()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (MapArray[x, y].BrickType == Brick.Type.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public WinType CheckWin()
        {
            if (CheckWinForBrick(Brick.Type.Circle))
            {
                return WinType.Circle;
            }
            else if (CheckWinForBrick(Brick.Type.Cross))
            {
                return WinType.Cross;
            }
            else if(CheckDraw())
            {
                return WinType.Draw;
            }
            else
            {
                return WinType.None;
            }
        }

        public bool SetBrick(int x, int y, Brick.Type brickType)
        {
            if (MapArray[x, y].BrickType == Brick.Type.None)
                MapArray[x, y].BrickType = brickType;
            else
                return false;
            return true;
        }
    }
}
