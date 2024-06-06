using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Result
    {
        //Attributes
        public Player Winner { get; }
        public EndCase Reason { get; }


        //Functions
        public Result(Player winner, EndCase reason)
        {
            Winner = winner;
            Reason = reason;
        }

        public static Result Win(Player winner)
        {
            return new Result(winner, EndCase.Checkmate);
        }

        public static Result Draw(EndCase reason)
        {
            return new Result(Player.None, reason);
        }
    }
}
