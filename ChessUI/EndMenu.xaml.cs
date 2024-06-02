//EndMenu

using ChessRules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for EndMenu.xaml
    /// </summary>
    public partial class EndMenu : UserControl
    {
        public event Action<Options> OptionsChanged;

        public EndMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            Winner.Text = GetTextWinner(result.Winner);
            Reason.Text = GetTextReason(result.Reason, gameState.CurrentTurn);
        }

        private static string GetTextWinner(Player w)
        {
            switch(w)
            {
                case Player.White:
                    return "WHITE WON!";
                case Player.Black:
                    return "BLACK WON!";
                default:
                    return "DRAW!";
                
            }
        }

        private static string StringPlayer(Player p)
        {
            switch (p)
            {
                case Player.White:
                    return "WHITE";
                case Player.Black:
                    return "BLACK";
                default:
                    return "";

            }
        }

        private static string GetTextReason(EndCase r, Player p)
        {
            switch(r)
            {
                case EndCase.Stalemate:
                    return $"STALEMATE - {StringPlayer(p)} CAN'T MOVE";
                case EndCase.Checkmate:
                    return $"CHECKMATE - {StringPlayer(p)} IS CHECKMATED";
                case EndCase.FiftyMovesRule:
                    return "FIFTY-MOVE RULE";
                default:
                    return "";
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsChanged != null)
            {
                OptionsChanged.Invoke(Options.Restart);
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsChanged != null)
            {
                OptionsChanged.Invoke(Options.Quit);
            }
        }
    }
}
