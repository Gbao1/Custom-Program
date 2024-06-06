using ChessRules;

public class PieceFactory
{
    public static PieceType GetPieceType(char c)
    {
        PieceType pieceType;

        switch (char.ToLower(c))
        {
            case 'p':
                pieceType = PieceType.Pawn;
                break;
            case 'n':
                pieceType = PieceType.Knight;
                break;
            case 'b':
                pieceType = PieceType.Bishop;
                break;
            case 'r':
                pieceType = PieceType.Rook;
                break;
            case 'q':
                pieceType = PieceType.Queen;
                break;
            case 'k':
                pieceType = PieceType.King;
                break;
            default:
                throw new ArgumentException($"Invalid piece type: {c}");
        }

        return pieceType;
    }

    public static Piece CreatePiece(PieceType pieceType, Player color, bool hasMoved)
    {
        Piece piece;

        switch (pieceType)
        {
            case PieceType.Pawn:
                piece = new Pawn(color);
                break;
            case PieceType.Knight:
                piece = new Knight(color);
                break;
            case PieceType.Bishop:
                piece = new Bishop(color);
                break;
            case PieceType.Rook:
                piece = new Rook(color);
                break;
            case PieceType.Queen:
                piece = new Queen(color);
                break;
            case PieceType.King:
                piece = new King(color);
                break;
            default:
                throw new ArgumentException($"Invalid piece type: {pieceType}");
        }

        if (hasMoved)
        {
            piece.MarkMoved();
        }

        return piece;
    }
}

