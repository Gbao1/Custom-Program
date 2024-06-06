//Castle.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Castle : Move
    {
        //Attributes
        private readonly Direction kingDir;
        private readonly Positions rookFrom;
        private readonly Positions rookTo;


        //Functions
        public override MoveType Type { get; }
        public override Positions From { get; }
        public override Positions To { get; }

        public Castle(MoveType type, Positions kingPos)
        {
            Type = type;
            From = kingPos;
            //If castle is king side
            if (type == MoveType.KSCastle)
            {
                //all piece in the same row as king
                kingDir = Direction.Right;
                To = new Positions(kingPos.Row, 6);
                rookFrom = new Positions(kingPos.Row, 7); //rook must be in this position to castle
                rookTo = new Positions(kingPos.Row, 5);
            }
            //If castle is queen side
            else if (type == MoveType.QSCastle)
            {
                //all piece in the same row as king
                kingDir = Direction.Left;
                To = new Positions(kingPos.Row, 2);
                rookFrom = new Positions(kingPos.Row, 0); //rook must be in this position to castle
                rookTo = new Positions(kingPos.Row, 3);
            }
        }

        //Move Rook and King
        public override void Execute(Board board)
        {
            new Normal(From, To).Execute(board);
            new Normal(rookFrom, rookTo).Execute(board);
        }

        //Special check IsLegal because castling needs more rules (a space btw king and rook cant be target,etc.)
        public override bool IsLegal(Board board)
        {
            Player p = board[From].Color; //Get king with color

            if (board.InCheck(p))
            {
                return false;
            }

            Board copyBoard = board.Copy();
            Positions kingCopy = From; //make copy of king

            //move king to the side (left if queen castle, vice versa), if king is checked in any square -> cant castle
            for (int i = 0; i < 3; i++)
            {   
                new Normal(kingCopy, kingCopy + kingDir).Execute(copyBoard);
                kingCopy += kingDir;

                if (copyBoard.InCheck(p))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
