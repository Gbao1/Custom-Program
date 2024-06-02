//DoublePawn.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class DoublePawn : Move
    {
        public override MoveType Type { get; }
        public override Positions From { get; }
        public override Positions To { get; }

        private readonly Positions skippedSquare;

        public DoublePawn(Positions from, Positions to)
        {
            From = from;
            To = to;
            skippedSquare = new Positions((from.Row + to.Row) / 2, from.Column);
        }

        public override void Excecute(Board board)
        {
            Player p = board[From].Color;
            board.SetPawnSkippedSquares(p, skippedSquare);
            new Normal(From, To).Excecute(board);
        }
    }
}
