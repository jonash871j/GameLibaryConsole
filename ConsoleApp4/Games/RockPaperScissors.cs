using Engine;
using RockPaperScissorsLogic;

namespace Games
{
    class RockPaperScissors : Game
    {
        private GameManager game = new GameManager();
        private Sprite sprRock = new Sprite("Sprite/rps_rock.ascspr");
        private Sprite sprScissors = new Sprite("Sprite/rps_scissors.ascspr");
        private Sprite sprPaper = new Sprite("Sprite/rps_paper.ascspr");

        public RockPaperScissors()
            : base("Rock Paper Scissors")
        {
        }

        void DrawCharacter(int x, int y, Move move)
        {
            switch (move)
            {
            case Move.Rock      : Draw.Sprite(x, y, sprRock);     break;
            case Move.Scissors  : Draw.Sprite(x, y, sprScissors); break;
            case Move.Paper     : Draw.Sprite(x, y, sprRock);     break;
            default:                                              break;
            }
        }

        public void DrawCharacters()
        {
            DrawCharacter(2, 4, game.Player);
            DrawCharacter(ConsoleEx.Width - 18, 4, game.Enemy);
        }
        private void DrawHud()
        {
            ConsoleEx.WriteLine(game.GetWinMessage());
        }

        private void UserInput()
        {
            if (Input.KeyPressed((Key)'R'))
            {
                game.SetMove(Move.Rock);
            }
            else if (Input.KeyPressed((Key)'S'))
            {
                game.SetMove(Move.Scissors);
            }
            else if(Input.KeyPressed((Key)'P'))
            {
                game.SetMove(Move.Paper);
            }
        }

        public override void Play()
        {
            ConsoleEx.Create(64, 32);
            ConsoleEx.SetFont("Terminal", 16, 16);

            while (!Input.KeyPressed(Key.ESCAPE))
            {
                UserInput();

                DrawCharacters();
                DrawHud();

            

                ConsoleEx.Update();
                ConsoleEx.Clear();
            }
        }
    }
}
