using Engine;
using TetrisLogic;

namespace Games
{
    public class Tetris : Game
    {
        private GameManager game = new GameManager();
        private byte[] colorMap =
        {
            Color.Get(0xb, 0x3),
            Color.Get(0x9, 0x1),
            Color.Get(0xe, 0x6),
            Color.Get(0x7, 0x8),
            Color.Get(0xa, 0x2),
            Color.Get(0xd, 0x5),
            Color.Get(0xc, 0x4),
         };

        public Tetris()
            : base("Tetris")
        {
        }

        private void UserInput()
        {
            if (Input.KeyPressed(Key.LEFT))
                game.Transform(x: Direction.Backward, y: Direction.None);

            if (Input.KeyPressed(Key.RIGHT))
                game.Transform(x: Direction.Forward, y: Direction.None);

            if (Input.KeyPressed(Key.UP))
                game.Rotate(Direction.Forward);

            if (Input.KeyStateDelayed(Key.DOWN, 200))
                game.Transform(x: Direction.None, y: Direction.Forward);

            if (Input.KeyPressed(Key.SPACE))
                game.Warp();

            if ((Input.KeyPressed(Key.NUMPAD0)) || (Input.KeyPressed(Key.INSERT)) || (Input.KeyPressed(Key.RETURN)))
                game.Hold();
        }

        private void DrawBorder(int x1, int y1, int x2, int y2)
        {
            Draw.Line(x1, y1, x1, y2, '│', Color.White);
            Draw.Line(x2, y1, x2, y2, '│', Color.White);
            Draw.Line(x1, y1, x2, y1, '─', Color.White);
            Draw.Line(x1, y2, x2, y2, '─', Color.White);
            ConsoleEx.WriteCharacter(x1, y1, '┌', Color.White);
            ConsoleEx.WriteCharacter(x2, y1, '┐', Color.White);
            ConsoleEx.WriteCharacter(x1, y2, '└', Color.White);
            ConsoleEx.WriteCharacter(x2, y2, '┘', Color.White);
        }

        private void DrawTetromino(Tetromino tetromino, ushort character, byte color, int xOff, int yOff, bool useTetrominoProperties = true)
        {
            for (int y = 0; y < tetromino.Height; y++)
            {
                for (int x = 0; x < tetromino.Width; x++)
                {
                    if (useTetrominoProperties)
                    {
                        if (tetromino[x, y] != Field.Empty)
                        {
                            if (y + tetromino.Y < 0)
                                continue;

                            ConsoleEx.WriteCharacter(x + tetromino.X + xOff, y + tetromino.Y + yOff, character, color);
                        }
                    }
                    else
                    {
                        if (tetromino.GetField(270, x, y) != Field.Empty)
                        {
                            ConsoleEx.WriteCharacter(x + xOff, y + yOff, character, color);
                        }
                    }
                }
            }
        }

        private void DrawContainer(Container container, int xOff, int yOff)
        {
            DrawBorder(xOff-1, yOff-1, xOff + 4, yOff + 4);
            DrawTetromino(
                container.Tetromino, 
                '■', 
                colorMap[(int)container.Tetromino.FieldType], 
                xOff + (4 / container.Tetromino.Width) - 1, 
                yOff + (4 / container.Tetromino.Height) - 1, 
                false
            );
        }

        private void DrawPlayer(int xOff, int yOff)
        {
            DrawTetromino(game.Ghost, '▒', 0xf, xOff, yOff);
            DrawTetromino(game.Controller, '■', colorMap[(int)game.Controller.FieldType], xOff, yOff);
        }

        private void DrawMap(int xOff, int yOff)
        {
            Map map = game.Map;

            DrawContainer(game.HoldContainer, xOff - 1 - 4, yOff + 2);
            DrawContainer(game.NextContainer, xOff + map.Width + 1, yOff + 2);
            DrawBorder(xOff - 1, yOff - 1, map.Width + xOff, map.Height + yOff);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if ((map[x, y] != Field.Empty) && (map[x, y] != Field.Out))
                    {
                        ConsoleEx.WriteCharacter(xOff + x, yOff + y, '■', colorMap[(int)map[x, y]]);
                    }
                }
            }
        }

        private void DrawHud(int xOff, int yOff)
        {
            ConsoleEx.WriteCoord(xOff, yOff + 22);
            ConsoleEx.WriteLine("SCORE", 0xf);
            ConsoleEx.WriteLine(game.Score.ToString("D10"), 0x7);
        }

        private void DrawGame(int xOff, int yOff)
        {
            DrawMap(xOff, yOff);
            DrawPlayer(xOff, yOff);
            DrawHud(xOff, yOff);

            ConsoleEx.Update();
            ConsoleEx.Clear();
        }

        public override void Play()
        {
            ConsoleEx.Create(42, 26);
            ConsoleEx.SetFont("Terminal", 40, 40);
            ConsoleEx.SetColorPalette(new ColorPalette("./Color/tetris.accpal"));

            game.IsPaused = false;

            while (!Input.KeyPressed(Key.ESCAPE))
            {
                UserInput();
                DrawGame(17, 2);
            }

            game.IsPaused = true;
        }
    }
}
