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
        //Attributes
        private readonly Direction forward;


        //Functions
        public override PieceType Type
        {
            get { return PieceType.Pawn; }
        }
        public override Player Color { get; }

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

        public override void MarkMoved()
        {
            HasMoved = true;
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        //if that move is legal
        private static bool LegalMoveTo(Positions pos, Board board)
        {
            return Board.Inside(pos) && board.Empty(pos); //Inside is static while Empty is not
        }

        //Can you capture
        private bool CanCapture(Positions pos, Board board)
        {
            if (!Board.Inside(pos) || board.Empty(pos))
            {
                return false;
            }

            return board[pos].Color != Color; //Can capture if piece isn't the same side
        }

        //Forward movement
        private IEnumerable<Move> FMoves(Positions from, Board board)
        {
            Positions oneSquareMove = from + forward;

            if (LegalMoveTo(oneSquareMove, board))
            {
                //end of the board black or white
                if (oneSquareMove.Row == 0 || oneSquareMove.Row == 7)
                {
                    foreach (Move promoMove in PromotionMoves(from, oneSquareMove))
                    {
                        yield return promoMove;
                    }
                }
                //if not end of board move normally
                else
                {
                    yield return new Normal(from, oneSquareMove);
                }

                //If pawn hasnt move then can move 2 square
                Positions twoSquareMove = oneSquareMove + forward;

                if (!HasMoved && LegalMoveTo(twoSquareMove, board))
                {
                    yield return new DoublePawn(from, twoSquareMove);
                }
            }
        }

        //Diagnal movement
        private IEnumerable<Move> Dmoves(Positions from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.Left, Direction.Right })
            {
                //1 square diagnal forward
                Positions to = from + forward + dir;

                //if that square is a pawn skipped square by opponent -> can make en passant
                if (to == board.GetPawnSkippedSquares(Color.Opponent()))
                {
                    yield return new EnPassant(from, to);
                }

                else if (CanCapture(to, board))
                {
                    //if after capture is end of board
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promoMove in PromotionMoves(from, to))
                        {
                            yield return promoMove;
                        }
                    }
                    //if not move normal
                    else
                    {
                        yield return new Normal(from, to);
                    }
                }
            }
        }

        //all possible moves
        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            return FMoves(from, board).Concat(Dmoves(from, board));
        }

        //Can capture opponent king dianally
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
            //For each possible promotion piece(Rook, Knight, Bishop, Queen), a new Promotion move is created and yielded.
            yield return new Promotion(from, to, PieceType.Rook);
            yield return new Promotion(from, to, PieceType.Knight);
            yield return new Promotion(from, to, PieceType.Bishop);
            yield return new Promotion(from, to, PieceType.Queen);
        }
    }
}
