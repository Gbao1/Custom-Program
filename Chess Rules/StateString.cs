//StateString.cs

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    /// <summary>
    /// a stateString is similar to a FEN string, rules are:
    /// It is devided into 4 parts:
    /// 1st 2nd 3r 4th, seperate by a space
    ///1st part is piece postions, the rules are:
    ///CAPITAL for White pieces
    ///lower for black pieces
    ///each row is seperate by "/"(a full string will have all 8 rows)
    ///the number represent how many empty squares are between each pieces: Example: 4k2r means: from the top left, 4 empty squares, next square is black king, next is 2 empty squares, next square is black rook
    ///2nd part is the current player turn: w for white and b for black
    ///3rd part is catsling rights: Q for queenside and K for king side, 
    ///again, CAPITAL is white rights and lowercase is black rights.
    ///so Qk means Queen side for white and k means king side for black.
    ///last part is en passant rights: Example: g3, this is the square that a black pawn could move to to do an en passant.
    /// </summary>
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public StateString(Player currentTurn, Board board)
        {
            AddPiecesPositions(board);
            sb.Append(' ');
            AddCurrentTurn(currentTurn);
            sb.Append(' ');
            AddCastling(board);
            sb.Append(' ');
            AddEnPassant(board, currentTurn);
        }

        private static string PiecesString(Piece piece)
        {
            char c;
            switch (piece.Type)
            {
                case PieceType.Pawn:
                    c = 'p';
                    break;
                case PieceType.Bishop:
                    c = 'b';
                    break;
                case PieceType.Knight:
                    c = 'n';
                    break;
                case PieceType.Rook:
                    c = 'r';
                    break;
                case PieceType.Queen:
                    c = 'q';
                    break;
                case PieceType.King:
                    c = 'k';
                    break;
                default:
                    c = ' ';
                    break;
            }

            if (piece.Color == Player.White)
            {
                c = char.ToUpper(c);
            }

            if (piece.HasMoved)
            {
                return c.ToString() + '+';
            }

            return c.ToString();
        }

        private void AddRow(Board board, int row)
        {
            int empty = 0;

            for (int i = 0; i < 8; i++)
            {
                if (board[row, i] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PiecesString(board[row, i]));
            }

            if (empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecesPositions(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0)
                {
                    sb.Append('/');
                }

                AddRow(board, row);
            }
        }

        private void AddCurrentTurn(Player currentTurn)
        {
            if (currentTurn == Player.White)
            {
                sb.Append('w');
            }
            else if (currentTurn == Player.Black)
            {
                sb.Append('b');
            }
        }

        private void AddCastling(Board board)
        {
            bool WKSCastle = board.KSCastleRight(Player.White);
            bool WQSCastle = board.QSCastleRight(Player.White);
            bool BKSCastle = board.KSCastleRight(Player.Black);
            bool BQSCastle = board.QSCastleRight(Player.Black);

            if (!(WKSCastle || WQSCastle || BKSCastle || BQSCastle))
            {
                sb.Append('-');
                return;
            }

            if (WKSCastle) sb.Append('K');
            if (WQSCastle) sb.Append('Q');
            if (BKSCastle) sb.Append('k');
            if (BQSCastle) sb.Append('q');
        }

        private void AddEnPassant(Board board, Player currentTurn)
        {
            if (!board.EnPassantRight(currentTurn))
            {
                sb.Append('-');
                return;
            }

            Positions pos = board.GetPawnSkippedSquares(currentTurn.Opponent());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            sb.Append(file).Append(rank);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

}
