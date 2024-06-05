﻿using System;
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
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public event Action<Options> OptionsChanged;

        public PauseMenu()
        {
            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsChanged != null)
            {
                OptionsChanged.Invoke(Options.Continue);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsChanged != null)
            {
                OptionsChanged.Invoke(Options.Save);
            }
        }
    }
}
