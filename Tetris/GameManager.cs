using System;
using System.Collections.Generic;
using System.Text;

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

        public Map Map { get; private set; }
        public Tetromino[] Tetrominos { get; private set; }
        public Tetromino Controller { get; private set; }
        

        public GameManager(int width = 10, int height = 21)
        {
            Map = new Map(width, height);
            Tetrominos = new Tetromino[]
            {
                new Tetromino(".X...X...X...X..", Field.Cyan),
                new Tetromino("..X...X..XX.....", Field.Blue),
                new Tetromino(".X...X...XX.....", Field.Orange),
                new Tetromino(".....XX..XX.....", Field.Yellow),
                new Tetromino(".X...XX...X.....", Field.Green),
                new Tetromino(".X...XX..X......", Field.Purple),
                new Tetromino("..X..XX..X......", Field.Red),
            };

            Controller = Tetrominos[0].Clone();
        }

        public void PickRandom()
        {
            Controller = Tetrominos[random.Next(0, Tetrominos.Length)].Clone();
        }

        /// <summary>
        /// Rotates the controller tetromino
        /// </summary>
        /// <returns>if the rotation was successful</returns>
        public bool Rotate(Direction direction)
        {
            Tetromino test = Controller.Clone();
            test.Rotate(direction);

            if (!Map.CheckTetrominoCollision(test))
            {
                Controller = test;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the controller tetromino
        /// </summary>
        /// <returns>if the transform was successful</returns>
        public bool Transform(Direction x, Direction y)
        {
            Tetromino test = Controller.Clone();
            test.Transform(x, y);
            
            if ((y != Direction.None) && (Map.CheckTetrominoCollision(test)))
            {
                Map.PlaceTetromino(Controller);
                return false;
            }

            if (!Map.CheckTetrominoCollision(test))
                Controller = test;

            return true;
        }

        /// <summary>
        /// Warps the controller tetromino until somthing is hit
        /// </summary>
        public void Warp()
        {
            while (Transform(Direction.None, Direction.Forward)) ;
        }
    }
}
