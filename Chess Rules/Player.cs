//Player.cs

namespace ChessRules
{
    public enum Player
    {
        None,
        Black,
        White
    }

    public static class ExtensionMethods
    {
        //"this" makes Opponent() an extension method for "Player" instances 
        public static Player Opponent(this Player p)
        {
            switch(p)
            {
                case Player.Black:
                    return Player.White;
                case Player.White:
                    return Player.Black;
                default:
                    return Player.None;
            }
        }
    }
}
