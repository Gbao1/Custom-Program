//GameState.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentTurn { get; private set; } //mutable in class but read-only outside
        public Result Result { get; private set; } = null; //mutable in class but read-only outside

        private string stateString;

        public GameState(Player player,  Board board)
        {
            CurrentTurn = player;
            Board = board;

            stateString = new StateString(CurrentTurn, board).ToString();
        }

        public IEnumerable<Move> LegalMoves(Positions pos)
        {   
            //If position clicked is empty square or opponent's piece, dont return any moves
            if (Board.Empty(pos) || Board[pos].Color != CurrentTurn)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            //Pass the position of the piece to GetMoves because the piece itself doesn't know where it is
            IEnumerable<Move> allMoves = piece.GetMoves(pos, Board);
            List<Move> legalMoves = new List<Move>();

            foreach (Move move in allMoves)
            {
                if (move.IsLegal(Board))
                {
                    legalMoves.Add(move);
                }
            }

            return legalMoves;
        }

        public void Moving(Move move)
        {
            Board.SetPawnSkippedSquares(CurrentTurn, null);
            move.Excecute(Board);
            CurrentTurn = CurrentTurn.Opponent(); //change turn
            StateStringUpdate();
            CheckEnded();
        }

        //Get all moves of all pieces of current player to check checkmate or stalemate
        public IEnumerable<Move> AllLegalMoves(Player p)
        {
            List<Move> allLegalMoves = new List<Move>();

            foreach (Positions pos in Board.PiecePosColor(p))
            {
                Piece piece = Board[pos];
                IEnumerable<Move> moves = piece.GetMoves(pos, Board);

                foreach (Move move in moves)
                {
                    if (move.IsLegal(Board))
                    {
                        allLegalMoves.Add(move);
                    }
                }
            }

            return allLegalMoves;
        }

        private void CheckEnded()
        {
            if (!AllLegalMoves(CurrentTurn).Any())
            {
                if (Board.InCheck(CurrentTurn))
                {
                    Result = Result.Win(CurrentTurn.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndCase.Stalemate);
                }
            }
        }

        public bool IsEnded()
        {
            return Result != null;
        }

        private void StateStringUpdate()
        {
            stateString = new StateString(CurrentTurn, Board).ToString();
        }

        public void Save(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);

            try
            {
                writer.WriteLine(stateString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving file: " + e.Message);
            }
            finally
            {
                writer.Close();
            }
        }

    }
}
