using ChessRules;

public static class PieceFactory
{
    public static PieceType GetPieceType(char c)
    {
        char pieceChar = char.ToUpper(c);
        PieceType pieceType;

        switch (pieceChar)
        {
            case 'P':
                pieceType = PieceType.Pawn;
                break;
            case 'N':
                pieceType = PieceType.Knight;
                break;
            case 'B':
                pieceType = PieceType.Bishop;
                break;
            case 'R':
                pieceType = PieceType.Rook;
                break;
            case 'Q':
                pieceType = PieceType.Queen;
                break;
            case 'K':
                pieceType = PieceType.King;
                break;
            default:
                throw new ArgumentException($"Invalid piece type: {c}");
        }

        return pieceType;
    }


    public static Piece CreatePiece(PieceType pieceType, Player color)
    {
        switch (pieceType)
        {
            case PieceType.Pawn:
                return new Pawn(color);
            case PieceType.Knight:
                return new Knight(color);
            case PieceType.Bishop:
                return new Bishop(color);
            case PieceType.Rook:
                return new Rook(color);
            case PieceType.Queen:
                return new Queen(color);
            case PieceType.King:
                return new King(color);
            default:
                throw new ArgumentException($"Invalid piece type: {pieceType}");
        }
    }
}