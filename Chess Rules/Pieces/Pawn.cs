//Pawn.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Pawn : Piece
    {
        public override PieceType Type
        {
            get { return PieceType.Pawn; }
        }
        public override Player Color { get; }

        private readonly Direction forward;

        public Pawn(Player color)
        {
            Color = color;

            if (color == Player.White)
            {
                forward = Direction.Up;
            }
            else if (color == Player.Black)
            {
                forward = Direction.Down;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool LegalMoveTo(Positions pos, Board board)
        {
            return Board.Inside(pos) && board.Empty(pos); //Inside is static while Empty is not
        }

        private bool CanCapture(Positions pos, Board board)
        {
            if (!Board.Inside(pos) || board.Empty(pos))
            {
                return false;
            }

            return board[pos].Color != Color; //Can capture if piece isn't the same side
        }

        private IEnumerable<Move> FMoves(Positions from, Board board)
        {
            Positions oneSquareMove = from + forward;

            if (LegalMoveTo(oneSquareMove, board))
            {
                if (oneSquareMove.Row == 0 || oneSquareMove.Row == 7)
                {
                    foreach (Move promoMove in PromotionMoves(from, oneSquareMove))
                    {
                        yield return promoMove;
                    }
                }
                else
                {
                    yield return new Normal(from, oneSquareMove);
                }

                Positions twoSquareMove = oneSquareMove + forward;

                if (!HasMoved && LegalMoveTo(twoSquareMove, board))
                {
                    yield return new DoublePawn(from, twoSquareMove);
                }
            }
        }

        private IEnumerable<Move> Dmoves(Positions from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.Left, Direction.Right })
            {
                Positions to = from + forward + dir;

                if (to == board.GetPawnSkippedSquares(Color.Opponent()))
                {
                    yield return new EnPassant(from, to);
                }

                else if (CanCapture(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promoMove in PromotionMoves(from, to))
                        {
                            yield return promoMove;
                        }
                    }
                    else
                    {
                        yield return new Normal(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            return FMoves(from, board).Concat(Dmoves(from, board));
        }

        public override bool CanCaptureKing(Positions from, Board board)
        {

            foreach (Move move in Dmoves(from, board))
            {
                Piece piece = board[move.To];
                if (piece != null && piece.Type == PieceType.King)
                {
                    return true;
                }    
            }

            return false;
        }

        private static IEnumerable<Move> PromotionMoves(Positions from, Positions to)
        {
            yield return new Promotion(from, to, PieceType.Rook);
            yield return new Promotion(from, to, PieceType.Knight);
            yield return new Promotion(from, to, PieceType.Bishop);
            yield return new Promotion(from, to, PieceType.Queen);
        }
    }
}
