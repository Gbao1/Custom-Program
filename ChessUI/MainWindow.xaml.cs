//MainWindow

using System.IO;
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
        //Attributes
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8]; //highlights the legal moves
        private readonly Dictionary<Positions, Move> moveStore = new Dictionary<Positions, Move>(); //store all the legal moves of the piece

        //I want to have a single, shared save file path across all instances of MainWindow -> static
        private static string SaveFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "saved_game.txt");

        private GameState gameState;
        private Positions selected = null; //store the selected position
        private Positions promoFrom;
        private Positions promoTo;


        //Functions

        /// <summary>
        /// How the piece moves: when player click the piece, store selected position in 'selected'
        /// then ask the GameState what are the legal moves of the piece (stored in moveStore using [x, y] as the key)
        /// those legal moves then get highlighted, when player click on one of the highlight, the corresponding move then execute.
        /// </summary>

        public MainWindow()
        {
            InitializeComponent();

            MainMenu mainMenuControl = new MainMenu();

            // Check if the save file exists
            bool saveFileExists = File.Exists(SaveFilePath);

            if (saveFileExists)
            {
                mainMenuControl.btnLoadGame.Visibility = Visibility.Visible;
            }
            else
            {
                mainMenuControl.btnLoadGame.Visibility= Visibility.Collapsed;
            }

            // Add the MainMenuUserControl to the MainMenuContainer
            MainMenuContainer.Content = mainMenuControl;

            // Wire up the button events to your existing event handlers
            mainMenuControl.btnNewGame.Click += btnNewGame_Click;
            mainMenuControl.btnLoadGame.Click += btnLoadGame_Click;
            mainMenuControl.btnQuitGame.Click += btnQuitGame_Click;

        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeGameState(true);
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeGameState(false);
        }

        private void InitializeGameState(bool isNewGame)
        {
            InitializedBoard();

            if (isNewGame)
            {
                gameState = new GameState(Player.White, Board.SetUp());
                Draw(gameState.Board);
            }
            else
            {
                if (File.Exists(SaveFilePath))
                {
                    Board loadedBoard = Board.LoadedBoard(SaveFilePath);
                    Player currentTurn = GetCurrentTurn(loadedBoard);
                    gameState = new GameState(currentTurn, loadedBoard);
                    Draw(gameState.Board);
                }
                else
                {
                    MessageBox.Show("Save file not found.");
                }
            }

            MainMenuContainer.Content = null;
            BoardGrid.Visibility = Visibility.Visible;
        }

        private Player GetCurrentTurn(Board board)
        {
            // Read the state string from the file
            string stateString;

            using (StreamReader reader = new StreamReader(SaveFilePath))
            {
                stateString = reader.ReadLine();
            }

            // Extract the character representing the current player's turn
            char turnChar = stateString.Split(' ')[1][0];

            // Determine the current player's turn based on the extracted character
            Player currentTurn;
            if (turnChar == 'w')
            {
                currentTurn = Player.White;
            }
            else
            {
                currentTurn = Player.Black;
            }

            return currentTurn;
        }

        private void btnQuitGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
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

        //The destination pos
        private void SelectedTo(Positions pos)
        {
            selected = null;
            UnHighlighting();

            if (moveStore.TryGetValue(pos, out Move move))
            {
                //if its a promotion move
                if (move.Type == MoveType.PawnPromotion)
                {   
                    promoFrom = move.From;
                    promoTo = move.To;
                    HandlePromotionMoves(promoFrom, promoTo);
                }
                //else normal move
                else
                {
                    HandleMoves(move);
                }
            }
        }

        //Handle promotion moves
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

        //Handle normal moves
        private void HandleMoves(Move move)
        {
            gameState.Moving(move);
            Draw(gameState.Board);

            if (gameState.Result != null)
            {
                //Show the end menu
                EndMenu menu = new EndMenu(gameState);
                MenuBox.Content = menu;
                menu.OptionsChanged += OnOptionsChanged; //subscribe to the Event
            }
        }

        //Storing legal moves in the moveStore dictionary.
        private void StoredMoves(IEnumerable<Move> moves)
        {
            moveStore.Clear();

            foreach (Move move in moves)
            {
                moveStore[move.To] = move;
            }
        }

        //Highlighting squares on the board to indicate legal move destinations.
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
                //Restart the game
                UnHighlighting();
                moveStore.Clear();
                gameState = new GameState(Player.White, Board.SetUp());
                Draw(gameState.Board);
            }
            else if (option == Options.Quit)
            {
                Application.Current.Shutdown();
            }
            else if (option == Options.Continue)
            {
                MenuBox.Content = null;
            }
            else if (option == Options.Save)
            {
                gameState.Save(SaveFilePath);
                Application.Current.Shutdown();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsMenuOn() && e.Key == Key.Escape)
            {
                //Show pause menu
                PauseMenu pauseMenu = new PauseMenu();
                MenuBox.Content = pauseMenu;
                pauseMenu.OptionsChanged += OnOptionsChanged;
            }
        }

    }
}