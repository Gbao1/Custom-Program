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
        //Attributes
        public abstract MoveType Type { get; }
        public abstract Positions From { get; }
        public abstract Positions To { get; }


        //Functions
        public abstract void Execute(Board board);

        //Need to make this virtual because when I add castling moves, there are more conditions to check.
        public virtual bool IsLegal(Board board)
        {
            //make a copy of board with pieces positions, move is legal if after moving the king isnt in check
            Player p = board[From].Color;
            Board copyBoard = board.Copy();
            Execute(copyBoard);
            return !copyBoard.InCheck(p);
        }
    }
}
