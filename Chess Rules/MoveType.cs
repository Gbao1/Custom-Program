using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public enum MoveType
    {
        Normal,
        KSCastle,
        QSCastle,
        EnPassant,
        PawnFirstMove,
        PawnPromotion
    }
}
