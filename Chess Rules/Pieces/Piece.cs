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

        public abstract void MarkMoved();

        //take a pos in a direction, if empty, keep adding until out of the board or meet a opponent piece, if so, break
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

        //Apply logic for 1 directions to multiple, depends on what piece, their dirs is different
        protected IEnumerable<Positions> MoveInDirs(Positions from, Board board, Direction[] dirs)
        {
            List<Positions> positions = new List<Positions>();

            foreach (Direction dir in dirs)
            {
                // adding all the positions returned by the MoveInDir to the positions list.
                positions.AddRange(MoveInDir(from, board, dir));
            }

            return positions;
        }

        //default implementation to check if can capture opponent's king
        public virtual bool CanCaptureKing(Positions from, Board board)
        {
            //check each legal moves
            foreach (Move move in GetMoves(from, board))
            {
                //if that legal pos has opponent king return true
                //(in legal moves checking we already considered our own piece as illegal so this will be opponet's king)
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
