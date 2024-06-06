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
        //Functions
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

        public override IEnumerable<Move> GetMoves(Positions from, Board board)
        {
            // Generate potential legal moves
            foreach (Direction vertDir in new Direction[] { Direction.Up, Direction.Down })
            {
                foreach (Direction horDir in new Direction[] { Direction.Left, Direction.Right })
                {
                    Positions[] potentialMoves = new Positions[]
                    {
                        //Create L shape move in all 4 directions (2 each so 8 potentials in total)
                        from + 2 * vertDir + horDir,
                        from + 2 * horDir + vertDir
                    };

                    // Check each potential move to see if it's legal
                    foreach (Positions to in potentialMoves)
                    {
                        if (Board.Inside(to) && (board.Empty(to) || board[to].Color != Color))
                        {
                            yield return new Normal(from, to);
                        }
                    }
                }
            }
        }

    }
}
