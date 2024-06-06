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
        //Attributes
        private readonly Positions captureSquare;


        //Functions
        public override MoveType Type { get; }
        public override Positions From { get; }
        public override Positions To { get; }

        public EnPassant(Positions from, Positions to)
        {
            From = from;
            To = to;
            captureSquare = new Positions(from.Row, to.Column);
        }

        //move the pawn and set capture square to null
        public override void Execute(Board board)
        {
            new Normal(From, To).Execute(board);
            board[captureSquare] = null;
        }
    }
}
