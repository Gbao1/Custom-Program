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
        public override MoveType Type { get; }
        public override Positions From { get; }
        public override Positions To { get; }

        private readonly Direction kingDir;
        private readonly Positions rookFrom;
        private readonly Positions rookTo;

        public Castle(MoveType type, Positions kingPos)
        {
            Type = type;
            From = kingPos;

            if (type == MoveType.KSCastle)
            {
                kingDir = Direction.Right;
                To = new Positions(kingPos.Row, 6);
                rookFrom = new Positions(kingPos.Row, 7);
                rookTo = new Positions(kingPos.Row, 5);
            }
            else if (type == MoveType.QSCastle)
            {
                kingDir = Direction.Left;
                To = new Positions(kingPos.Row, 2);
                rookFrom = new Positions(kingPos.Row, 0);
                rookTo = new Positions(kingPos.Row, 3);
            }
        }

        public override void Excecute(Board board)
        {
            new Normal(From, To).Excecute(board);
            new Normal(rookFrom, rookTo).Excecute(board);
        }

        public override bool IsLegal(Board board)
        {
            Player p = board[From].Color;

            if (board.InCheck(p))
            {
                return false;
            }

            Board copyBoard = board.Copy();
            Positions kingCopy = From;

            for (int i = 0; i < 3; i++)
            {   
                new Normal(kingCopy, kingCopy + kingDir).Excecute(copyBoard);
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
