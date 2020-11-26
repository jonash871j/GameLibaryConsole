using Engine;
using TetrisLogic;

namespace Games
{
    public class Tetris : Game
    {
        private GameManager game;
        private Sprite[] sprTetrominos;
        private Sprite sprGhost;

        public Tetris()
            : base("Tetris")
        {
            game = new GameManager();
            sprGhost = new Sprite("./Sprite/t_ghost.ascspr");
            sprTetrominos = new Sprite[]
            {
                new Sprite("./Sprite/t_tetrominoCyan.ascspr"),
                new Sprite("./Sprite/t_tetrominoBlue.ascspr"),
                new Sprite("./Sprite/t_tetrominoOrange.ascspr"),
                new Sprite("./Sprite/t_tetrominoYellow.ascspr"),
                new Sprite("./Sprite/t_tetrominoGreen.ascspr"),
                new Sprite("./Sprite/t_tetrominoPurple.ascspr"),
                new Sprite("./Sprite/t_tetrominoRed.ascspr"),
            };
        }

        void DrawTetromino(Tetromino tetromino, Sprite sprite, int xOff, int yOff, bool useTetrominoCoord = true)
        {
            for (int y = 0; y < tetromino.Height; y++)
            {
                for (int x = 0; x < tetromino.Width; x++)
                {
                    if (tetromino[x, y] != Field.Empty)
                    {
                        if (useTetrominoCoord)
                            Draw.Sprite((x * 3) + (tetromino.X * 3) + xOff, (y * 3) + (tetromino.Y * 3) + yOff, sprite);
                        else
                            Draw.Sprite((x * 3) + xOff, (y * 3) + yOff, sprite);
                    }
                }
            }
        }
        void DrawMap(Map map, int xOff, int yOff)
        {
            Draw.Rectangle(xOff-2, yOff-2, map.Width * 3 + xOff+1, map.Height * 3 + yOff+1, true, '█', Color.White);
                        DrawContainer(game.HoldContainer, xOff - 15, yOff + 4);
            DrawContainer(game.NextContainer, game.Map.Width * 3 + xOff + 3, yOff + 4);

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if ((map[x, y] != Field.Empty) && (map[x, y] != Field.Out))
                    {
                        Draw.Sprite(x * 3 + xOff, y * 3 + yOff, sprTetrominos[(int)map[x, y]]);
                    }
                }
            }
        }
        void DrawContainer(Container container, int xOff, int yOff)
        {
            Draw.Rectangle(xOff-2, yOff-2, 13 + xOff, 13 + yOff, true, '█', Color.White);
            DrawTetromino(container.Tetromino, sprTetrominos[(int)container.Tetromino.FieldType], xOff, yOff, false);
        }
        private void UserInput()
        {
            if (Input.KeyStateDelayed(Key.LEFT, 35))
                game.Transform(x: Direction.Backward, y: Direction.None);

            if (Input.KeyStateDelayed(Key.RIGHT, 35))
                game.Transform(x: Direction.Forward, y: Direction.None);

            if (Input.KeyPressed(Key.UP))
                game.Rotate(Direction.Forward);

            if (Input.KeyStateDelayed(Key.DOWN, 35))
                game.Transform(x: Direction.None, y: Direction.Forward);

            if (Input.KeyPressed(Key.SPACE))
                game.Warp();

            if ((Input.KeyPressed(Key.NUMPAD0)) || (Input.KeyPressed(Key.INSERT)) || (Input.KeyPressed(Key.RETURN)))
                game.Hold();
        }

        public override void Play()
        {
            ConsoleEx.Create(128, 80);
            ConsoleEx.SetFont("Terminal", 8, 8);
            ConsoleEx.SetColorPalette(new ColorPalette("./Color/tetris.accpal"));

            game.IsPaused = false;

            while (!Input.KeyPressed(Key.ESCAPE))
            {
                UserInput();

                DrawTetromino(game.Ghost, sprGhost, 32, 2);
                DrawTetromino(game.Controller, sprTetrominos[(int)game.Controller.FieldType], 32, 2);
                DrawMap(game.Map, 32, 2);
            
                ConsoleEx.WriteCoord(32, 75);
                ConsoleEx.WriteLine(game.Score.ToString());

                ConsoleEx.Update();
                ConsoleEx.Clear();
            }

            game.IsPaused = true;
        }
    }
}
