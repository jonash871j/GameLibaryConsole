namespace Games
{
    public abstract class Game
    {
        public string Name { get; set; }

        protected Game(string name)
        {
            Name = name;
        }

        public abstract void Play();
    }
}
