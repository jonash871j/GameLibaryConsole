using Engine;
using TicTacToeLogic;

namespace Games
{
    public class TicTacToe : Game
    {
        private GameManager game = new GameManager(size: 3);
        private int xCus = 0, yCus = 0;

        public TicTacToe() 
            : base("Tic Tac Toe")
        {
        }

        void DrawX(int x, int y)
        {
            ConsoleEx.WriteCoord(x * 5 + 6, y * 5);
            ConsoleEx.WriteLine("X   X", 0xc);
            ConsoleEx.WriteLine(" X X ", 0xc);
            ConsoleEx.WriteLine("  X  ", 0xc);
            ConsoleEx.WriteLine(" X X ", 0xc);
            ConsoleEx.WriteLine("X   X", 0xc);
        }
        private void DrawO(int x, int y)
        {
            ConsoleEx.WriteCoord(x * 5 + 6, y * 5);
            ConsoleEx.WriteLine(" OOO ", 0x9);
            ConsoleEx.WriteLine("O   O", 0x9);
            ConsoleEx.WriteLine("O   O", 0x9);
            ConsoleEx.WriteLine("O   O", 0x9);
            ConsoleEx.WriteLine(" OOO ", 0x9);
        }
        private void DrawCursor(int x, int y)
        {
            ConsoleEx.WriteCoord(x * 5 + 6, y * 5);
            ConsoleEx.WriteLine("■ ■ ■", 0xa);
            ConsoleEx.WriteLine("     ", 0xa);
            ConsoleEx.WriteLine("■   ■", 0xa);
            ConsoleEx.WriteLine("     ", 0xa);
            ConsoleEx.WriteLine("■ ■ ■", 0xa);
        }
        private void DrawMap(Map map)
        {
            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++)
                {
                    if (map.MapArray[x, y].BrickType == Brick.Type.Cross)
                        DrawX(x, y);
                    else if (map.MapArray[x, y].BrickType == Brick.Type.Circle)
                        DrawO(x, y);
                }
            }
        }
        private void DrawHud()
        {
            ConsoleEx.WriteCoord(2, (game.Map.Size * 5) + 1, "");
            ConsoleEx.WriteLine(game.GetWinMessage());
            ConsoleEx.WriteLine(game.GetBrickMessage());
        }

        private void UserInput()
        {
            if (Input.KeyPressed(Key.UP))
                yCus--;
            if (Input.KeyPressed(Key.DOWN))
                yCus++;
            if (Input.KeyPressed(Key.LEFT))
                xCus--;
            if (Input.KeyPressed(Key.RIGHT))
                xCus++;

            if (Input.KeyPressed(Key.RETURN))
            {
                if (game.CheckWin() == WinType.None)
                    game.SetBrick(xCus, yCus);
                else
                    game.Reset();
            }

            if (xCus < 0)
                xCus = 0;
            if (xCus >= game.Map.Size - 1)
                xCus = game.Map.Size - 1;

            if (yCus < 0)
                yCus = 0;
            if (yCus >= game.Map.Size - 1)
                yCus = game.Map.Size - 1;
        }

        public override void Play()
        {
            ConsoleEx.Create((short)(5 * game.Map.Size + 12), (short)(5 * game.Map.Size + 3));
            ConsoleEx.SetFont("Terminal", 16, 16);

            while (!Input.KeyPressed(Key.ESCAPE))
            {
                UserInput();

                DrawMap(game.Map);
                DrawCursor(xCus, yCus);
                DrawHud();

                ConsoleEx.Update();
                ConsoleEx.Clear();
            }
        }
    }
}
