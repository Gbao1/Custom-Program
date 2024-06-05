//Queen.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Queen : Piece
    {
        public override PieceType Type
        {
            get { return PieceType.Queen; }
        }
        public override Player Color { get; }

        private static readonly Direction[] dirs = new Direction[]
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

        public Queen(Player color)
        {
            Color = color;
        }

        public override void MarkMoved()
        {
            HasMoved = true;
        }

        public override Piece Copy()
        {
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        //get all legal moves in all directions
        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            List<Move> moves = new List<Move>();
            IEnumerable<Positions> reachablePositions = MoveInDirs(from, board, dirs);

            foreach (Positions to in reachablePositions)
            {
                moves.Add(new Normal(from, to));
            }

            return moves;
        }
    }
}
