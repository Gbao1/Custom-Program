//PromoMenu

using ChessRules;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PromoMenu.xaml
    /// </summary>
    public partial class PromoMenu : UserControl
    {
        public event Action<PieceType> SelectedPiece;

        public PromoMenu(Player p)
        {
            InitializeComponent();

            QueenImage.Source = Images.GetImage(p, PieceType.Queen);
            KnightImage.Source = Images.GetImage(p, PieceType.Knight);
            BishopImage.Source = Images.GetImage(p, PieceType.Bishop);
            RookImage.Source = Images.GetImage(p, PieceType.Rook);
        }

        private void QueenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.Invoke(PieceType.Queen);
            }
        }

        private void KnightImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.Invoke(PieceType.Knight);
            }
        }

        private void BishopImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.Invoke(PieceType.Bishop);
            }
        }

        private void RookImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.Invoke(PieceType.Rook);
            }
        }
    }
}
