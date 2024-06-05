//Knight.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Knight : Piece
    {
        public override PieceType Type
        {
            get { return PieceType.Knight; }
        }
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override void MarkMoved()
        {
            HasMoved = true;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static IEnumerable<Positions> PotentialLegalMoves(Positions from)
        {
            foreach (Direction vertDir in new Direction[] { Direction.Up, Direction.Down })
            {
                foreach(Direction horDir in new Direction[] {Direction.Left, Direction.Right})
                {
                    yield return from + 2 * vertDir + horDir;
                    yield return from + 2 * horDir + vertDir;
                }
            }
        }

        private IEnumerable<Positions> ActualLegalMoves(Positions from, Board board)
        {
            List<Positions> legalMoves = new List<Positions>();

            foreach (Positions pos in PotentialLegalMoves(from))
            {
                if (Board.Inside(pos) && (board.Empty(pos) || board[pos].Color != Color))
                {
                    legalMoves.Add(pos);
                }
            }

            return legalMoves;
        }

        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            List<Move> moves = new List<Move>();
            IEnumerable<Positions> legalPositions = ActualLegalMoves(from, board);

            foreach (Positions to in legalPositions)
            {
                moves.Add(new Normal(from, to));
            }

            return moves;
        }
    }
}
