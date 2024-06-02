//Move.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Positions From { get; }
        public abstract Positions To { get; }

        public abstract void Excecute(Board board); //Command pattern

        //Need to make this virtual because when I add castling moves, there are more conditions to check.
        public virtual bool IsLegal(Board board)
        {
            Player p = board[From].Color;
            Board copyBoard = board.Copy();
            Excecute(copyBoard);
            return !copyBoard.InCheck(p);
        }
    }
}
