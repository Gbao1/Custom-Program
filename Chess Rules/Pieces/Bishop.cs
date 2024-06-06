//Bishop.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Bishop : Piece
    {
        //Attributes
        public static readonly Direction[] dirs = new Direction[]
        {
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight
        };


        //Functions
        public override PieceType Type
        {
            get { return PieceType.Bishop; }
        }
        public override Player Color { get; }

        public Bishop(Player color)
        {
            Color = color;
        }

        public override void MarkMoved()
        {
            HasMoved = true;
        }

        public override Piece Copy()
        {
            Bishop copy = new Bishop(Color);
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
