using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        //Because piece don't store their positions on the board so we need current position as a parameter.
        public abstract IEnumerable<Move> GetMoves(Positions from, Board board); //piece can return all legal moves.

        protected IEnumerable<Positions> MoveInDir(Positions from, Board board, Direction dir)
        {
            for (Positions pos = from + dir; Board.Inside(pos); pos += dir)
            {
                if (board.Empty(pos))
                {
                    yield return pos;
                    //make MoveInDir a iterator method (return sequence of value, 1 at a time)
                    continue;
                }

                Piece piece = board[pos];

                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }

        protected IEnumerable<Positions> MoveInDirs(Positions from, Board board, Direction[] dirs)
        {
            List<Positions> positions = new List<Positions>();

            foreach (Direction dir in dirs)
            {
                positions.AddRange(MoveInDir(from, board, dir));
            }

            return positions;
        }

        public virtual bool CanCaptureKing(Positions from, Board board)
        {

            foreach (Move move in GetMoves(from, board))
            {
                Piece piece = board[move.To];
                if (piece != null && piece.Type == PieceType.King)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
