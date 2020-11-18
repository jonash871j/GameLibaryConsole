using System;

namespace TicTacToe
{
    public enum WinType
    {
        None,
        Cross,
        Circle,
        Draw,
    }

    public class GameManager
    {
        public Map Map { get; }
        public Brick.Type BrickTurn { get; private set; }

        public GameManager(int size)
        {
            Map = new Map(size);
            SetRandomBrickTurn();
        }

        private void SetRandomBrickTurn()
        {
            Random random = new Random();

            if (random.Next(0, 1) == 0)
                BrickTurn = Brick.Type.Cross;
            else
                BrickTurn = Brick.Type.Circle;
        }

        public void Reset()
        {
            Map.Reset();
        }

        public WinType CheckWin()
        {
            return Map.CheckWin();
        }

        public void SetBrick(int x, int y)
        {
            if (Map.SetBrick(x, y, BrickTurn))
            {
                // Switches turn
                if (BrickTurn == Brick.Type.Circle)
                    BrickTurn = Brick.Type.Cross;
                else
                    BrickTurn = Brick.Type.Circle;
            }
        }

        public string GetWinMessage()
        {
            WinType type = Map.CheckWin();
            switch (type)
            {
            case WinType.Cross:
                return "X is the winer!";
            case WinType.Circle:
                return "O is the winer!";
            case WinType.Draw:
                return "It's a draw!";
            default:
                return "The game is still active...";
            }
        }
        public string GetBrickMessage()
        {
            return $"It's { BrickTurn }'s turn";
        }
    }
}
