//Images.cs

using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessRules;

namespace ChessUI
{
    public static class Images
    {
        //Attributes
        private static readonly Dictionary<PieceType, ImageSource> SourcesW = new()
        {
            {PieceType.Pawn, Load("Images/PawnW.png") },
            {PieceType.Knight, Load("Images/KnightW.png") },
            {PieceType.Bishop, Load("Images/BishW.png") },
            {PieceType.Rook, Load("Images/RookW.png") },
            {PieceType.Queen, Load("Images/QueenW.png") },
            {PieceType.King, Load("Images/KingW.png") }
        };

        private static readonly Dictionary<PieceType, ImageSource> SourcesB = new()
        {
            {PieceType.Pawn, Load("Images/PawnB.png") },
            {PieceType.Knight, Load("Images/KnightB.png") },
            {PieceType.Bishop, Load("Images/BishB.png") },
            {PieceType.Rook, Load("Images/RookB.png") },
            {PieceType.Queen, Load("Images/QueenB.png") },
            {PieceType.King, Load("Images/KingB.png") }
        };


        //Functions

        //Load the image using BitMap
        private static ImageSource Load(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        //Get the corressponding piece's image
        //If you have the Player and PieceType values available separately,
        //you can use this GetImage(Player color, PieceType type) method directly.
        public static ImageSource GetImage(Player color, PieceType type)
        {
            switch (color)
            {
                case Player.White:
                    return SourcesW[type];
                case Player.Black:
                    return SourcesB[type];
                default:
                    return null;
            }
        }

        //If you have an instance of the Piece class, you can use this GetImage(Piece piece) method,
        //which internally calls the other GetImage method with the appropriate color and type.
        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }

            return GetImage(piece.Color, piece.Type);
        }
    }
}
