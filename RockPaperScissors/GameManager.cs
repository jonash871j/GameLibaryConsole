using System;

namespace RockPaperScissorsLogic
{
    public enum WinType
    {
        None,
        Win,
        Lose,
        Draw,
    }
    public enum Move
    {
        Rock = 0,
        Scissors = 1,
        Paper = 2,
        Unset,
    }

    public class GameManager
    {
        public Move Player { get; private set; }
        public Move Enemy { get; private set; }
        private Random random = new Random();

        public GameManager()
        {
            Reset();
        }

        public void Reset()
        {
            Player = Move.Unset;
            Enemy = Move.Unset;
        }
        public void SetMove(Move move)
        {
            Player = move;
            Enemy = (Move)random.Next(0, 3);
        }
        public WinType CheckWin()
        {
            if (Player == Move.Unset)
                return WinType.None;

            if ((Player == Move.Rock) && (Enemy == Move.Scissors))
                return WinType.Win;
            else if ((Player == Move.Scissors) && (Enemy == Move.Paper))
                return WinType.Win;
            else if ((Player == Move.Paper) && (Enemy == Move.Rock))
                return WinType.Win;
            else if (Player == Enemy)
                return WinType.Draw;
            else
                return WinType.Lose;
        }
        public string GetWinMessage()
        {
            switch (CheckWin())
            {
            case WinType.None   : return "The game is still active!";
            case WinType.Win    : return "You won the game!";
            case WinType.Lose   : return "You lost the game!";
            case WinType.Draw   : return "It's a draw!";
            default             : return "Error...";
            }
        }
    }
}
