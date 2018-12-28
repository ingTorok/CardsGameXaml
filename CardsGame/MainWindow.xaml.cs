using CardsGame.Models;
using CardsGame.WPF;
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

        /// <summary>
        /// Handeling KeyDown Events for left, right, and down
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (ButtonNewGame.IsVisible == true & e.Key == Key.Up)
            {
                GameSpace = new GameSpace(this);
                return;
            }
            else

            if (!GameSpace.IsGame & ButtonStart.IsVisible == true & e.Key == Key.Up)
            {
                GameSpace.StartCountDown();
                return;
            }

            if (GameSpace.IsGame)
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

        /// <summary>
        /// Creating new (= new GameSpace objekt)
        /// </summary>
        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            GameSpace = new GameSpace(this);
        }

        /// <summary>
        /// Start Game
        /// </summary>
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            GameSpace.StartCountDown();
        }

        /// <summary>
        /// Handeling click on "No", "Yes" Buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (GameSpace.IsGame)
            {
                switch (btn.Name.ToString())
                {
                    case "ButtonNo":
                        GameSpace.KeyDown(Key.Left);
                        break;
                    //case "ButtonPartially":
                    //    break;
                    case "ButtonYes":
                        GameSpace.KeyDown(Key.Right);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
