namespace TicTacToeLogic
{
    public class Brick
    {
        public enum Type
        {
            None,
            Circle,
            Cross,
        }
        public Type BrickType { get; set; }

        public Brick(Type brickType)
        {
            BrickType = brickType;
        }
    }
}
