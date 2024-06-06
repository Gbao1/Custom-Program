//Promotion.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Promotion : Move
    {
        //Attribute
        private readonly PieceType promotedType;


        //Functions
        public override MoveType Type { get { return MoveType.PawnPromotion; } }

        public override Positions From { get; }
        public override Positions To { get; }

        public Promotion(Positions from, Positions to, PieceType promotedType)
        {
            From = from;
            To = to;
            this.promotedType = promotedType;
        }

        //move the pawn, set the pawn to whatever been promoted to
        public override void Execute(Board board)
        {
            Piece pawn = board[From];
            board[From] = null;

            //ensure promoted piece is the same color as the pawn
            Piece promoted = PieceFactory.CreatePiece(promotedType, pawn.Color, true);
            board[To] = promoted;
        }
    }
}
