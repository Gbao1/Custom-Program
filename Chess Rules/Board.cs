//Board.cs

using System;
using System.Collections.Generic;
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
            board.AddInitialPieces();
            return board;
        }

        

        private void AddInitialPieces()
        {
            //Black pieces first row
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            //White pieces first row
            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int a = 0; a < 8; a++)
            {
                this[1, a] = new Pawn(Player.Black); //Black pawns
                this[6, a] = new Pawn(Player.White); //White pawns
            }
        }

        public static Board LoadedBoard(string filePath)
        {
            string stateStringText = File.ReadAllText(filePath);
            Board board = new Board();
            board.LoadPieces(stateStringText);
            return board;
        }

        private void LoadPieces(string stateString)
        {
            // Split the state string into its components
            string[] parts = stateString.Split(' ');

            // Load the piece positions (1st part)
            string[] rows = stateString.Split('/');
            for (int row = 0; row < 8; row++)
            {
                string rowString = rows[row];
                int col = 0;
                foreach (char c in rowString)
                {
                    if (char.IsDigit(c))
                    {
                        col += int.Parse(c.ToString());
                    }
                    else
                    {
                        Player color = Player.Black;
                        if (char.IsUpper(c))
                        {
                            color = Player.White;
                        }
                        
                        // Check if the piece has moved based on the marker "*"
                        bool hasMoved = false;
                        if (c == '*')
                        {
                            hasMoved = true;
                        }

                        PieceType pieceType = PieceFactory.GetPieceType(c);
                        // Create the piece
                        Piece piece = PieceFactory.CreatePiece(pieceType, color);

                        // Set the "HasMoved" property of the piece
                        if (piece != null)
                        {
                            if (hasMoved)
                            {
                                piece.HasMoved = true;
                            }
                        }

                        // Assign the piece to the board
                        this[row, col] = piece;
                        col++;
                    }
                }
            }

            // Set the current player turn (2nd part)
            string turn = parts[1];
            // You might want to handle this information as needed in your application

            // Set castling rights (3rd part)
            string castlingRights = parts[2];
            // You might want to handle this information as needed in your application

            // Set en passant square (4th part)
            string enPassantSquare = parts[3];
            // You might want to handle this information as needed in your application
        }


        public static bool Inside(Positions pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool Empty(Positions pos)
        {
            return this[pos] == null;
        }

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

        public bool QSCastleRight(Player p)
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

        public bool KSCastleRight(Player p)
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
        private bool HasPawn(Player p, Positions[] pawnPos, Positions skippedPos)
        {
            foreach (Positions pos in  pawnPos.Where(Inside))
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

        //check if you can en passant
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

            return HasPawn(p, pawnPos, skippedPos);
        }

    }
}
