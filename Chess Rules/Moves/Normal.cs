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
        public override MoveType Type {  get { return MoveType.Normal; } }
        public override Positions From { get; }
        public override Positions To { get; }

        public Normal(Positions from, Positions to)
        {
            From = from;
            To = to;
        }

        public override void Excecute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            piece.HasMoved = true;
        }
    }
}
