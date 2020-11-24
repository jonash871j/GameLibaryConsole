using Engine;
using TicTacToeLogic;

namespace Games
{
    public class TicTacToe : Game
    {
        private GameManager game = new GameManager(size: 3);
        private Sprite sprCross = new Sprite("Sprite/ttt_cross.ascspr");
        private Sprite sprCircle = new Sprite("Sprite/ttt_circle.ascspr");
        private Sprite sprCursor = new Sprite("Sprite/ttt_cursor.ascspr");
        private int xCus = 0, yCus = 0;

        public TicTacToe() 
            : base("Tic Tac Toe")
        {
        }

        private void DrawMap(Map map)
        {
            // Draws map
            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++)
                {
                    if (map.MapArray[x, y].BrickType == Brick.Type.Cross)
                        Draw.Sprite(x * 5 + 6, y * 5, sprCross);
                    else if (map.MapArray[x, y].BrickType == Brick.Type.Circle)
                        Draw.Sprite(x * 5 + 6, y * 5, sprCircle);
                }
            }

            // Draws cursor
            Draw.Sprite(xCus * 5 + 6, yCus * 5, sprCursor);
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
                DrawHud();

                ConsoleEx.Update();
                ConsoleEx.Clear();
            }
        }
    }
}
