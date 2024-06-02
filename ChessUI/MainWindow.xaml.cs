//MainWindow

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessRules;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8]; //highlights the legal moves
        private readonly Dictionary<Positions, Move> moveStore = new Dictionary<Positions, Move>(); //store all the legal moves of the piece
            
        private GameState gameState;
        private Positions selected = null; //store the selected position
        private Positions promoFrom;
        private Positions promoTo;
        /// <summary>
        /// How the piece moves: when player click the piece, store selected position in 'selected'
        /// then ask the GameState what are the legal moves of the piece (stored in moveStore using [x, y] as the key)
        /// those legal moves then get highlighted, when player click on one of the highlight, the corresponding move then execute.
        /// </summary>

        public MainWindow()
        {
            InitializeComponent();
            InitializedBoard();

            gameState = new GameState(Player.White, Board.SetUp());
            Draw(gameState.Board);
        }

        private void InitializedBoard()
        {
            for (int a = 0; a < 8; a ++)
            {
                for (int b = 0; b < 8; b++)
                {
                    Image image = new Image();
                    pieceImages[a, b] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[a, b] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
            
        }

        private void Draw(Board board)
        {
            for (int a = 0;a < 8; a ++)
            {
                for(int b = 0;b < 8; b++)
                {
                    Piece piece = board[a, b];
                    pieceImages[a, b].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //If player click when menu is on (game is over) then nothing happens
            if (IsMenuOn())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Positions pos = ToSquare(point);

            if (selected == null)
            {
                SelectedFrom(pos);
            }
            else
            {
                SelectedTo(pos);
            }
        }

        private Positions ToSquare(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int col = (int)(point.X / squareSize);
            int row = (int)(point.Y / squareSize);
            return new Positions(row, col);

        }

        private void SelectedFrom(Positions pos)
        {
            IEnumerable<Move> moves = gameState.LegalMoves(pos);

            if (moves.Any())
            {
                selected = pos;
                StoredMoves(moves);
                Highlighting();
            }
        }

        private void SelectedTo(Positions pos)
        {
            selected = null;
            UnHighlighting();

            if (moveStore.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {   
                    promoFrom = move.From;
                    promoTo = move.To;
                    HandlePromotionMoves(promoFrom, promoTo);
                }
                else
                {
                    HandleMoves(move);
                }
            }
        }

        private void HandlePromotionMoves(Positions from, Positions to)
        {
            // Show the pawn at to position as a step between promoting
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentTurn, PieceType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            PromoMenu promoMenu = new PromoMenu(gameState.CurrentTurn);
            MenuBox.Content = promoMenu;

            // Subscribe to the SelectedPiece event
            promoMenu.SelectedPiece += OnPieceSelected;
        }

        // Event handler method for piece promotion selection
        private void OnPieceSelected(PieceType type)
        {
            MenuBox.Content = null;
            Move promoMove = new Promotion(promoFrom, promoTo, type);
            HandleMoves(promoMove);
        }


        private void HandleMoves(Move move)
        {
            gameState.Moving(move);
            Draw(gameState.Board);

            if (gameState.IsEnded())
            {
                ShowMenu();
            }
        }

        private void StoredMoves(IEnumerable<Move> moves)
        {
            moveStore.Clear();

            foreach (Move move in moves)
            {
                moveStore[move.To] = move;
            }
        }

        private void Highlighting()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Positions to in moveStore.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void UnHighlighting()
        {
            foreach (Positions to in moveStore.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private bool IsMenuOn()
        {
            return MenuBox.Content != null;
        }

        //Event handler method
        private void OnOptionsChanged(Options option)
        {
            if (option == Options.Restart)
            {
                MenuBox.Content = null;
                GameRestart();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void ShowMenu()
        {
            EndMenu menu = new EndMenu(gameState);
            MenuBox.Content = menu;
            menu.OptionsChanged += OnOptionsChanged; //subscribe to the Event
        }


        private void GameRestart()
        {
            UnHighlighting();
            moveStore.Clear();
            gameState = new GameState(Player.White, Board.SetUp());
            Draw(gameState.Board);
        }
    }
}