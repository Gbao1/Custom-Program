//Normal.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Normal : Move
    {
        //Functions
        public override MoveType Type {  get { return MoveType.Normal; } }
        public override Positions From { get; }
        public override Positions To { get; }

        public Normal(Positions from, Positions to)
        {
            From = from;
            To = to;
        }

        //move the piece and set piece as HasMoved, set From pos to null (so UI wont draw at old pos)
        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            piece.HasMoved = true;
        }
    }
}
