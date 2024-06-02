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
        public override MoveType Type { get { return MoveType.PawnPromotion; } }

        public override Positions From { get; }
        public override Positions To { get; }

        private readonly PieceType promotedType;

        public Promotion(Positions from, Positions to, PieceType promotedType)
        {
            From = from;
            To = to;
            this.promotedType = promotedType;
        }

        private Piece PromotionPieces(Player color)
        {
            switch(promotedType)
            {
                case PieceType.Rook:
                    return new Rook(color);
                case PieceType.Knight:
                    return new Knight(color);
                case PieceType.Bishop:
                    return new Bishop(color);
                default:
                    return new Queen(color);
            }
        }

        public override void Excecute(Board board)
        {
            Piece pawn = board[From];
            board[From] = null;

            Piece promoted = PromotionPieces(pawn.Color);
            promoted.HasMoved = true;
            board[To] = promoted;
        }
    }
}
