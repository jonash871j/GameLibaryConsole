using System;
using System.Timers;

namespace TetrisLogic
{
    public enum Direction
    {
        Backward = -1,
        None = 0,
        Forward = 1,
    }

    public enum Field
    {
        Empty = -2,
        Out = -1,
        Cyan = 0,
        Blue = 1,
        Orange = 2,
        Yellow = 3,
        Green = 4,
        Purple = 5,
        Red = 6,
    }

    public class GameManager
    {
        private Random random = new Random();
        private Timer timer;
        private bool isTransformDownMade = false;
        private bool isNewHoldMade = false;

        public Map Map { get; private set; }
        public Tetromino[] Tetrominos { get; private set; }
        public Tetromino Controller { get; private set; }
        public Tetromino Ghost { get; private set; }
        public Container HoldContainer { get; private set; }
        public Container NextContainer { get; private set; }
        public int Score { get; private set; }
        public bool IsPaused { get; set; } = true;

        /// <summary>
        /// Used to create a game of tetris
        /// </summary>
        /// <param name="timerInterval">in milliseconds</param>
        /// <param name="width">width of the map</param>
        /// <param name="height">height of the map</param>
        public GameManager(int timerInterval = 500, int width = 10, int height = 21)
        {
            timer = new Timer(timerInterval);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;

            Map = new Map(width, height);
            Tetrominos = new Tetromino[]
            {
                new Tetromino("....XXXX........", Field.Cyan, 90, 4, 4),
                new Tetromino("...XXX..X", Field.Blue, 180),
                new Tetromino("...XXXX..", Field.Orange, 180),
                new Tetromino("XXXX", Field.Yellow, 0, 2, 2),
                new Tetromino("....XXXX.", Field.Green),
                new Tetromino("...XXX.X.", Field.Purple, 180),
                new Tetromino("...XX..XX", Field.Red),
            };

            Reset();
        }

        /// <summary>
        /// Used to update ghost position
        /// </summary>
        private void UpdateGhost()
        {
            Ghost = Controller.Clone();

            Tetromino test = Ghost;

            while (!Map.CheckPlaceCollision(test))
            {
                Ghost = test;
                test = Ghost.Clone();
                test.Transform(0, Direction.Forward);
            }
        }

        /// <summary>
        /// Used to move tetromino controller on each timed inteval
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!IsPaused)
            {
                if (!isTransformDownMade)
                    Transform(Direction.None, Direction.Forward);

                isTransformDownMade = false;
            }
        }

        /// <summary>
        /// Picks next tetromino for controler 
        /// </summary>
        private void Next()
        {
            Tetromino randomTetromino = Tetrominos[random.Next(0, Tetrominos.Length)].Clone();

            if (NextContainer.IsEmpty)
            {
                NextContainer.Swap(randomTetromino);
                Next();
            }
            else
            {
                Controller = NextContainer.Swap(randomTetromino);
                Controller.Reset(Map.Width / 2 - 2, -Controller.Height);

                UpdateGhost();
            }
        }

        /// <summary>
        /// Used to reset the game
        /// </summary>
        public void Reset()
        {
            isNewHoldMade = false;
            Score = 0;

            HoldContainer = new Container();
            NextContainer = new Container();
            Map.Clear();

            Next();
        }

        /// <summary>
        /// Used to rotate the tetromino controller
        /// </summary>
        /// <returns>if the rotation was successful</returns>
        public bool Rotate(Direction direction)
        {
            Tetromino test = Controller.Clone();
            test.Rotate(direction);

            if ((!Map.CheckPlaceCollision(test)) && (!Map.CheckWallCloision(test)))
            {
                Controller = test;
                UpdateGhost();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Used to move the tetromino controller
        /// </summary>
        /// <returns>if the transform was successful</returns>
        public bool Transform(Direction x, Direction y)
        {
            Tetromino test = Controller.Clone();
            test.Transform(x, y);
            
            if (y != Direction.None)
            {
                isTransformDownMade = true;

                if (Map.CheckPlaceCollision(test))
                {
                    // Checks for lose
                    if (Controller.Y < 0)
                    {
                        Reset();
                    }
                    else // Place tetromino
                    {
                        isNewHoldMade = false;
                        Map.PlaceTetromino(Controller);
                        Score += Map.ClearLines() * 100;
                        Next();

                    }
                    return false;
                }
            }

            if ((!Map.CheckPlaceCollision(test)) && (!Map.CheckWallCloision(test)))
                Controller = test;

            UpdateGhost();

            return true;
        }

        /// <summary>
        /// Used to warp the tetromino controller until somthing is hit
        /// </summary>
        public void Warp()
        {
            while (Transform(Direction.None, Direction.Forward)) ;
        }

        /// <summary>
        /// Used to swap controler with hold container
        /// </summary>
        public void Hold()
        {
            if (!isNewHoldMade)
            {
                if (HoldContainer.IsEmpty)
                {
                    HoldContainer.Swap(Controller);
                    Next();
                }
                else
                {
                    Controller = HoldContainer.Swap(Controller);
                    Controller.Reset(Map.Width / 2 - 2, -Controller.Height);
                }
            }
            isNewHoldMade = true;
        }
    }
}
