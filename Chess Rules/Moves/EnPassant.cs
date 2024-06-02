//EnPassant.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class EnPassant : Move
    {
        public override MoveType Type { get; }
        public override Positions From { get; }
        public override Positions To { get; }

        private readonly Positions captureSquare;

        public EnPassant(Positions from, Positions to)
        {
            From = from;
            To = to;
            captureSquare = new Positions(from.Row, to.Column);
        }

        public override void Excecute(Board board)
        {
            new Normal(From, To).Excecute(board);
            board[captureSquare] = null;
        }
    }
}
