using CardsGame.Models;
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

namespace CardsGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameSpace GameSpace;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            GameSpace = new GameSpace(this);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                //case Key.Down:
                    GameSpace.KeyDown(e.Key);
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
