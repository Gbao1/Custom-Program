//Board.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Positions> pawnSkippedSquares = new Dictionary<Player, Positions>
        {
            { Player.White, null },
            { Player.Black, null }
        };

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Positions pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public Positions GetPawnSkippedSquares(Player player)
        {
            return pawnSkippedSquares[player];
        }

        public void SetPawnSkippedSquares(Player p, Positions pos)
        {
            pawnSkippedSquares[p] = pos;
        }

        public static Board SetUp()
        {
            Board board = new Board();
            // Black pieces first row
            board[0, 0] = PieceFactory.CreatePiece(PieceType.Rook, Player.Black, false);
            board[0, 1] = PieceFactory.CreatePiece(PieceType.Knight, Player.Black, false);
            board[0, 2] = PieceFactory.CreatePiece(PieceType.Bishop, Player.Black, false);
            board[0, 3] = PieceFactory.CreatePiece(PieceType.Queen, Player.Black, false);
            board[0, 4] = PieceFactory.CreatePiece(PieceType.King, Player.Black, false);
            board[0, 5] = PieceFactory.CreatePiece(PieceType.Bishop, Player.Black, false);
            board[0, 6] = PieceFactory.CreatePiece(PieceType.Knight, Player.Black, false);
            board[0, 7] = PieceFactory.CreatePiece(PieceType.Rook, Player.Black, false);

            // White pieces first row
            board[7, 0] = PieceFactory.CreatePiece(PieceType.Rook, Player.White, false);
            board[7, 1] = PieceFactory.CreatePiece(PieceType.Knight, Player.White, false);
            board[7, 2] = PieceFactory.CreatePiece(PieceType.Bishop, Player.White, false);
            board[7, 3] = PieceFactory.CreatePiece(PieceType.Queen, Player.White, false);
            board[7, 4] = PieceFactory.CreatePiece(PieceType.King, Player.White, false);
            board[7, 5] = PieceFactory.CreatePiece(PieceType.Bishop, Player.White, false);
            board[7, 6] = PieceFactory.CreatePiece(PieceType.Knight, Player.White, false);
            board[7, 7] = PieceFactory.CreatePiece(PieceType.Rook, Player.White, false);

            for (int a = 0; a < 8; a++)
            {
                board[1, a] = PieceFactory.CreatePiece(PieceType.Pawn, Player.Black, false); // Black pawns
                board[6, a] = PieceFactory.CreatePiece(PieceType.Pawn, Player.White, false); // White pawns
            }

            return board;
        }


        public static Board LoadedBoard(string filePath)
        {
            string stateString = File.ReadAllText(filePath);
            string[] parts = stateString.Split(' ');

            Board board = new Board();

            // Load the piece positions
            string[] rows = parts[0].Split('/');
            for (int row = 0; row < 8; row++)
            {
                string rowString = rows[row];
                int col = 0;
                for (int i = 0; i < rowString.Length; i++)
                {
                    char c = rowString[i];

                    if (char.IsDigit(c))
                    {
                        col += int.Parse(c.ToString());
                    }
                    else
                    {
                        bool hasMoved = false;
                        if (i + 1 < rowString.Length && rowString[i + 1] == '+')
                        {
                            hasMoved = true;
                            i++; // Skip the '+'
                        }

                        Player color = Player.Black;
                        if (char.IsUpper(c))
                        {
                            color = Player.White;
                        }
                        PieceType pieceType = PieceFactory.GetPieceType(c);
                        // Create the piece with the hasMoved flag
                        Piece piece = PieceFactory.CreatePiece(pieceType, color, hasMoved);
                        // Assign the piece to the board
                        board[row, col] = piece;
                        col++;
                    }
                }
            }

            // Load en passant information if available
            if (parts.Length > 3)
            {
                string enPassantSquare = parts[3];
                if (enPassantSquare != "-")
                {
                    int row = enPassantSquare[1] - '1';
                    int col = enPassantSquare[0] - 'a';
                    Positions enPassantPos = new Positions(row, col);

                    if (row == 2) // White's skipped square (rank 3)
                    {
                        board.SetPawnSkippedSquares(Player.White, enPassantPos);
                    }
                    else if (row == 5) // Black's skipped square (rank 6)
                    {
                        board.SetPawnSkippedSquares(Player.Black, enPassantPos);
                    }
                }
            }

            return board;
        }

        //if that position is inside the board
        public static bool Inside(Positions pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        //If that position if empty square
        public bool Empty(Positions pos)
        {
            return this[pos] == null;
        }

        //All positions of all colors
        public IEnumerable<Positions> PiecesPos()
        {
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    Positions pos = new Positions(a, b);
                    yield return pos;
                }
            }
        }

        //All positions containing a piece of a certain color
        public IEnumerable<Positions> PiecePosColor(Player p)
        {
            foreach (Positions pos in PiecesPos())
            {
                Piece piece = this[pos];
                if (piece != null && piece.Color == p)
                {
                    yield return pos;
                }
            }
        }

        public bool InCheck(Player p)
        {
            //Get opponent pieces
            foreach (Positions pos in PiecePosColor(p.Opponent()))
            {
                Piece piece = this[pos];
                if (piece.CanCaptureKing(pos, this))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// We need to create a copy of a board because with our current legal move checking algo,
        /// When the player move, a copy of a board will be created, then the move will be excecuted in the board copy,
        /// If after the move, the current player's king isn't in check, then that move is legal.
        /// </summary>
        public Board Copy()
        {
            Board copy = new Board();

            foreach (Positions pos in PiecesPos())
            {
                //check if the piece is null or not
                Piece piece = this[pos];
                if (piece != null)
                {
                    //if not null, get a copy of the piece
                    Piece pieceCopy = piece.Copy();
                    if (pieceCopy != null)
                    {
                        //if not null, assign the position to the copied board
                        copy[pos] = pieceCopy;
                    }
                }
            }

            return copy;
        }

        private bool HasKingAndRookMoved(Positions kingPos, Positions rookPos)
        {
            if (Empty(rookPos) || Empty(kingPos))
            {
                return false;
            }

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;
        }

        public bool KSCastleRight(Player p)
        {
            switch (p)
            {
                case Player.White:
                    return HasKingAndRookMoved(new Positions(7, 4), new Positions(7, 7));
                case Player.Black:
                    return HasKingAndRookMoved(new Positions(0, 4), new Positions(0, 7));
                default:
                    return false;
            }
        }

        public bool QSCastleRight(Player p)
        {
            switch (p)
            {
                case Player.White:
                    return HasKingAndRookMoved(new Positions(7, 4), new Positions(7, 0));
                case Player.Black:
                    return HasKingAndRookMoved(new Positions(0, 4), new Positions(0, 0));
                default:
                    return false;
            }
        }

        //Check if there is a pawn in the position and if its legal
        public bool EnPassantRight(Player p)
        {
            Positions skippedPos = GetPawnSkippedSquares(p.Opponent());

            if (skippedPos == null)
            {
                return false;
            }

            Positions[] pawnPos;
            switch (p)
            {
                case Player.White:
                    pawnPos = new Positions[] { skippedPos + Direction.DownRight, skippedPos + Direction.DownLeft };
                    break;

                case Player.Black:
                    pawnPos = new Positions[] { skippedPos + Direction.UpRight, skippedPos + Direction.UpLeft };
                    break;

                default:
                    return false;
            }

            foreach (Positions pos in pawnPos.Where(Inside))
            {
                Piece piece = this[pos];
                if (piece == null || piece.Color != p || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(pos, skippedPos);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
