using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Controls;
using FontAwesome.WPF;

namespace CardsGame.Models
{
    /// <summary>
    /// This class is responsible for the game operation
    /// </summary>
    class GameSpace
    {  
        /// <summary>
        /// The MainWindow where the game is running
        /// </summary>
        private MainWindow MainWindow;
        
        /// <summary>
        /// Remaining time from the actually game
        /// </summary>
        private TimeSpan RemainedTime;

        /// <summary>
        /// Remainend time to start
        /// </summary>
        private TimeSpan StartTime;

        /// <summary>
        /// Reached points
        /// </summary>
        private int TotalPoints = 0;

        /// <summary>
        /// Show the Reaction Time
        /// </summary>
        private string Streak="       ";

        /// <summary>
        /// Variable to count Points
        /// </summary>
        private int StreakMultiplier = 0;

        /// <summary>
        /// Countdowner for the game
        /// </summary>
        private DispatcherTimer PendulumClock;

        /// <summary>
        /// Countdowner for start the game
        /// </summary>
        private DispatcherTimer CountDownClock;

        /// <summary>
        /// Gametime. Set to 45 sec
        /// </summary>
        private double GameTime = 45;

        /// <summary>
        /// Random for picking the cards to show
        /// </summary>
        private Random RandomNumber = new Random();

        /// <summary>
        /// Variable to hold the Card before
        /// </summary>
        private FontAwesomeIcon CardBefore = FontAwesomeIcon.None;

        /// <summary>
        /// Show if the game is Running or not
        /// </summary>
        public bool IsGame = false;

        /// <summary>
        /// Constructor, on creation get the MainWindow
        /// </summary>
        /// <param name="mainWindow">Window, which is showed on game progress</param>
        public GameSpace(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            SetNewGameCounters();          
            CardDeck = new CardDeck();
            EnableButtons();
            ShowNewCard();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartGame()
        {
            IsGame = true;
            SetPendulumClock();
            ShowNewCard();
            MainWindow.ViewboxStartCountDown.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartCountDown()
        {
            StartTime = TimeSpan.FromSeconds(3);
            MainWindow.ViewboxStartCountDown.Visibility = Visibility.Visible;
            MainWindow.ButtonStart.IsEnabled = false;
            SetCountDownClock();
        }

        /// <summary>
        /// CardDeck, here are stored the cards
        /// </summary>
        public CardDeck CardDeck { get; private set; }

        /// <summary>
        /// Set the game counters for Start.
        /// </summary>
        private void SetNewGameCounters()
        {
            RemainedTime = TimeSpan.FromSeconds(GameTime);
            ShowGameCounters();
        }

        /// <summary>
        /// show and refresh the gama counters
        /// </summary>
        private void ShowGameCounters()
        {
            MainWindow.LabelCountDown.Content = $"Time remaining: {RemainedTime.ToString("ss")}";
            MainWindow.LabelPoints.Content = $"Points: {TotalPoints}";
            MainWindow.LabelStreak.Content = $"Streak: {Streak.Substring(Streak.Length-7)}";
        }

        /// <summary>
        /// Enable Yes/No/Partially Buttons
        /// </summary>
        private void EnableButtons()
        {
            MainWindow.ButtonNewGame.IsEnabled = false;
            MainWindow.ButtonNewGame.Visibility = Visibility.Hidden;
            MainWindow.ButtonStart.Visibility = Visibility.Visible;
            MainWindow.ButtonStart.IsEnabled = true;
            MainWindow.ButtonNo.IsEnabled = true;
            MainWindow.ButtonYes.IsEnabled = true;
            //MainWindow.ButtonPartially.IsEnabled = true;
        }

        /// <summary>
        /// Disable Yes/No/Partially Buttons
        /// </summary>
        private void DisableButtons()
        {
            MainWindow.ButtonStart.IsEnabled = false;
            MainWindow.ButtonStart.Visibility = Visibility.Hidden;
            MainWindow.ButtonNewGame.Visibility = Visibility.Visible;
            MainWindow.ButtonNewGame.IsEnabled = true;
            MainWindow.ButtonNo.IsEnabled = false;
            MainWindow.ButtonYes.IsEnabled = false;
            //MainWindow.ButtonPartially.IsEnabled = false;
        }

        /// <summary>
        /// Set the Time to 45 sec
        /// </summary>
        private void SetPendulumClock()
        {
            PendulumClock = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, PendulumClockTicks, Application.Current.Dispatcher);
        }

        /// <summary>
        /// This function will countdown in every sec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendulumClockTicks(object sender, EventArgs e)
        {
            RemainedTime = RemainedTime.Add(TimeSpan.FromSeconds(-1));
            if (RemainedTime == TimeSpan.Zero)
            {
                GameOver();
            }
            ShowGameCounters();
        }

        private void SetCountDownClock()
        {
            CountDownClock = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Send, CountDownClockTicks, Application.Current.Dispatcher);
        }

        /// <summary>
        /// Write on the Mainwindow the actually remained time to start.
        /// </summary>
        private void CountDownClockTicks(object sender, EventArgs e)
        {
            //Transfotm the time in seconds and give back the last charachter (time to strat = 3, always <9)
            string timeUntylStart = StartTime.ToString("ss").Substring(StartTime.ToString("ss").Length - 1);

            //Write the actually tiem untyl start in the TextBlock
            MainWindow.TextBlockStartCountDown.Text = timeUntylStart;

            if (StartTime <= TimeSpan.FromSeconds(0))
            {//If the time untyl start is 0 then stop countdown and start the game
                CountDownClock.Stop();
                StartGame();
            }
            else
            {//Animation to micrify the Viewbox. It's use the Grid's Height - this property is responsive,
                var animationHight = new DoubleAnimation(MainWindow.GridStartCountDown.ActualHeight, 0, TimeSpan.FromMilliseconds(850));
                MainWindow.ViewboxStartCountDown.BeginAnimation(Viewbox.HeightProperty, animationHight);
                StartTime = StartTime.Add(TimeSpan.FromSeconds(-1));
            }
        }


        /// <summary>
        /// Picking up a new card
        /// </summary>
        private void ShowNewCard()
        {
            CardBefore = MainWindow.CardPlaceRight.Icon;
            Random random = new Random();
            var number = random.Next(0, CardDeck.NrOfCardsInPlay);
            TurnDownCard();
            MainWindow.CardPlaceRight.Icon = CardDeck.Cards[number];
        }

        private void TurnDownCard()
        {
            var animationDown = new DoubleAnimation(1, 0.15, TimeSpan.FromMilliseconds(65));           
            animationDown.Completed += TurnUpCard;
            MainWindow.imageScale.BeginAnimation(ScaleTransform.ScaleXProperty, animationDown);

        }

        private void TurnUpCard(object sender, EventArgs e)
        {
            var animationUp = new DoubleAnimation(0.15, 1, TimeSpan.FromMilliseconds(65));
            MainWindow.imageScale.BeginAnimation(ScaleTransform.ScaleXProperty, animationUp);
        }   

        /// <summary>
        /// Getting the Keydown event from the MainWindow
        /// </summary>
        /// <param name="key"></param>
        public void KeyDown(Key key)
        {
            switch (key)
            {
                case Key.Left:
                    NoAnswer();
                    break;
                case Key.Right:
                    YesAnswer();
                    break;
                //case Key.Down:
                //PartiallyAnswer;
                    //break;
                default:
                    throw new Exception($"Not acceptable Key: {key}");
            };
        }

        /// <summary>
        /// Picking the YES Answer/Button
        /// </summary>
        private void YesAnswer()
        {
            if (CardBefore == MainWindow.CardPlaceRight.Icon)
            {
                TheAnswerIsGood();
            }
            else
            {
                TheAnswerIsBad();
            }

            ShowNewCard();
        }

        /// <summary>
        /// Picking the NO Answer/Button
        /// </summary>
        private void NoAnswer()
        {
            if (CardBefore != MainWindow.CardPlaceRight.Icon)
            {
                TheAnswerIsGood();
            }
            else
            {
                TheAnswerIsBad();
            }

            ShowNewCard();
        }

        /// <summary>
        /// This will evaluate if the Answer is GOOD
        /// </summary>
        private void TheAnswerIsGood()
        {
            MainWindow.CardPlaceLeft.Icon = FontAwesomeIcon.Check;
            MainWindow.CardPlaceLeft.Foreground = Brushes.Green;
            HidingAnswer();
            CountStreak(true);
            CountPoints();
            ShowGameCounters();
        }

        /// <summary>
        /// This will evaluate if the Answer is BAD
        /// </summary>
        private void TheAnswerIsBad()
        {
            MainWindow.CardPlaceLeft.Icon = FontAwesomeIcon.Times;
            MainWindow.CardPlaceLeft.Foreground = Brushes.Red;
            HidingAnswer();
            CountStreak(false);
            CountPoints();
            ShowGameCounters();
        }

        /// <summary>
        /// Effect for Hiding Icons
        /// </summary>
        private void HidingAnswer()
        {
            var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            MainWindow.CardPlaceLeft.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// Counting the Streak for good and bad answers
        /// </summary>
        /// <param name="v"></param>
        private void CountStreak(bool v)
        {
            if (v)
            {
                Streak += "1";

                if (StreakMultiplier <= 0)
                {
                    StreakMultiplier = 0;
                }

                StreakMultiplier++;
            }
            else
            {
                Streak += "0";

                if (StreakMultiplier >= 0)
                {
                    StreakMultiplier = 0;
                }
               
                StreakMultiplier --;
            }

        }

        /// <summary>
        /// Method to count Points
        /// </summary>
        private void CountPoints()
        {
            if (StreakMultiplier < -3)
            {
                TotalPoints += -30;
            }
            else if (StreakMultiplier > 7)
            {
                TotalPoints += 70;
            }
            else
            {
                TotalPoints += StreakMultiplier * 10;
            }
        }

        /// <summary>
        /// Game Over
        /// </summary>
        private void GameOver()
        {
            PendulumClock.Stop();
            DisableButtons();
            IsGame = false;
        }
    }
}
