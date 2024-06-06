//King.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class King : Piece
    {
        //Attributes
        public readonly static Direction[] dirs = new Direction[]
        {
            Direction.Up,
            Direction.Right,
            Direction.Left,
            Direction.Down,
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight
        };


        //Functions
        public override PieceType Type
        {
            get { return PieceType.King; }
        }
        public override Player Color { get; }

        public King(Player color)
        {
            Color = color;
        }

        public override void MarkMoved()
        {
            HasMoved = true;
        }

        //Check if rook has moved
        private static bool HasRookMoved(Positions pos, Board board)
        {
            if (board.Empty(pos))
            {
                return false;
            }

            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        //Check if all squares are empty
        private static bool AllEmpty(IEnumerable<Positions> positions, Board board)
        {
            foreach (Positions pos in positions)
            {
                if (!board.Empty(pos))
                {
                    return false;
                }
            }
            return true;
        }

        //check if can castle king side
        private bool CanKSCastle(Positions from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Positions rookPos = new Positions(from.Row, 7);
            Positions[] squareBtw = new Positions[] { new(from.Row, 5), new(from.Row, 6) };

            return HasRookMoved(rookPos, board) && AllEmpty(squareBtw, board); //true if no piece between and rook not moved
        }

        //check if can castle queen side
        private bool CanQSCastle(Positions from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Positions rookPos = new Positions(from.Row, 0);
            Positions[] squareBtw = new Positions[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };

            return HasRookMoved(rookPos, board) && AllEmpty(squareBtw, board); //true if no piece between and rook not moved
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Positions> LegalMoves(Positions from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Positions to = from + dir;

                if (!Board.Inside(to))
                {
                    continue;
                }

                if (board.Empty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            foreach (Positions to in LegalMoves(from, board))
            {
                yield return new Normal(from, to);
            }

            if (CanKSCastle(from, board))
            {
                yield return new Castle(MoveType.KSCastle, from);
            }

            if (CanQSCastle(from, board))
            {
                yield return new Castle(MoveType.QSCastle, from);
            }
        }

        public override bool CanCaptureKing(Positions from, Board board)
        {
            foreach (Positions to in LegalMoves(from, board))
            {
                Piece piece = board[to];
                if (piece != null && piece.Type == PieceType.King)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
