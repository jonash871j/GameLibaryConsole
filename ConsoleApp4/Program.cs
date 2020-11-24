using Engine;
using Games;

class Program
{
    static void UpdateWindow()
    {
        ConsoleEx.Create(64, 32);
        ConsoleEx.SetFont("Terminal", 16, 16);
    }

    static void Main(string[] args)
    {
        Game[] games = new Game[]
        {
            new TicTacToe(),
            new RockPaperScissors(),
        };

        UpdateWindow();

        while (true)
        {
            ConsoleEx.WriteLine("- Game libary\n", Color.Purple);

            for (int i = 0; i < games.Length; i++)
            {
                ConsoleEx.Write("PRESS '" + (char)('A' + i) + "' TO PLAY ");
                ConsoleEx.WriteLine(games[i].Name, Color.Teal);

                if (Input.KeyPressed((Key)('A' + i)))
                {
                    games[i].Play();
                    UpdateWindow();
                }
            }
            ConsoleEx.WriteLine("\nPRESS 'ESCAPE' TO LEAVE GAME");
            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }
}