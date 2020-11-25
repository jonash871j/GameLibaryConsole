using Engine;
using TetrisLogic;

namespace Games
{
    public class Tetris : Game
    {
        private GameManager game = new GameManager();
        private Sprite[] sprTetrominos = new Sprite[]
        {
            new Sprite("./Sprite/t_tetrominoCyan.ascspr"),
            new Sprite("./Sprite/t_tetrominoBlue.ascspr"),
            new Sprite("./Sprite/t_tetrominoOrange.ascspr"),
            new Sprite("./Sprite/t_tetrominoYellow.ascspr"),
            new Sprite("./Sprite/t_tetrominoGreen.ascspr"),
            new Sprite("./Sprite/t_tetrominoPurple.ascspr"),
            new Sprite("./Sprite/t_tetrominoRed.ascspr"),
        };

        public Tetris()
            : base("Tetris")
        {
        }

        void DrawTetromino(Tetromino tetromino, int xOff, int yOff)
        {
            for (int y = 0; y < tetromino.Height; y++)
            {
                for (int x = 0; x < tetromino.Width; x++)
                {
                    if (tetromino[x, y] != Field.Empty)
                        Draw.Sprite((x * 3) + (tetromino.X * 3) + xOff, (y * 3) + (tetromino.Y * 3) + yOff, sprTetrominos[(int)tetromino.FieldType]);
                }
            }
            ConsoleEx.WriteLine(tetromino.X.ToString());
            ConsoleEx.WriteLine(tetromino.Y.ToString());
        }
        void DrawMap(Map map, int xOff, int yOff)
        {
            Draw.Rectangle(xOff-2, yOff-2, map.Width * 3 + xOff+1, map.Height * 3 + yOff+1, true, '█', Color.White);

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


        private void UserInput()
        {
            if (Input.KeyPressed((Key)'R'))
                game.PickRandom();

            if (Input.KeyPressed(Key.LEFT))
                game.Transform(x: Direction.Backward, y: Direction.None);

            if (Input.KeyPressed(Key.RIGHT))
                game.Transform(x: Direction.Forward, y: Direction.None);

            if (Input.KeyPressed(Key.UP))
                game.Rotate(Direction.Forward);

            if (Input.KeyPressed(Key.DOWN))
                game.Transform(x: Direction.None, y: Direction.Forward);


            if (Input.KeyPressed(Key.SPACE))
                game.Warp();
        }

        public override void Play()
        {
            ConsoleEx.Create(128, 80);
            ConsoleEx.SetFont("Terminal", 8, 8);
            ConsoleEx.SetColorPalette(new ColorPalette("./Color/tetris.accpal"));

            while (!Input.KeyPressed(Key.ESCAPE))
            {
                UserInput();

                DrawTetromino(game.Controller, 32, 2);
                DrawMap(game.Map, 32, 2);

                ConsoleEx.Update();
                ConsoleEx.Clear();
            }
        }
    }
}
