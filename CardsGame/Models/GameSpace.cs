﻿using System;
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
using CardsGame.WPF;
using System.Diagnostics;

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
        /// Countdowner for the game
        /// </summary>
        private DispatcherTimer PendulumClock;

        /// <summary>
        /// Countdowner for start the game
        /// </summary>
        private DispatcherTimer CountDownClock;

        /// <summary>
        /// Variable to hold the reactiontime
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// Gametime. Set to 30 sec
        /// </summary>
        private double GameTime = 30;

        /// <summary>
        /// Random for picking the cards to show
        /// </summary>
        private Random RandomNumber = new Random();

        /// <summary>
        /// Variable to hold the Card before
        /// </summary>
        private FontAwesomeIcon CardBefore = FontAwesomeIcon.None;

        /// <summary>
        /// Class to hold the Gamecounters
        /// </summary>
        private GameCounters GameCounters = new GameCounters();

        /// <summary>
        /// Show if the game is Running or not
        /// </summary>
        public bool IsGame = false;

        /// <summary>
        /// Show if is countdown or not
        /// </summary>
        public bool isCountdown = false;

        /// <summary>
        /// Top Scores
        /// </summary>
        HighScoreWindow TopScores;

        /// <summary>
        /// Constructor, on creation get the MainWindow
        /// </summary>
        /// <param name="mainWindow">Window, which is showed on game progress</param>
        public GameSpace(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            SetNewGameCounters();          
            CardDeck = new CardDeck();
            stopwatch = new Stopwatch();
            EnableButtons();
            ShowNewCard();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartGame()
        {
            IsGame = true;
            isCountdown = false;
            SetPendulumClock();
            ShowNewCard();
            stopwatch.Restart();
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
            MainWindow.LabelPoints.Content = $"Points: {GameCounters.TotalPoints}";

            var streak = GameCounters.Streak;
            MainWindow.LabelStreak.Content = $"Streak: {streak.Substring(streak.Length-7)}";
        }

        /// <summary>
        /// Enable Yes/No/Partially Buttons and set the visibility for Start(true) and New Game(false) buttons
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
        /// Disable Yes/No/Partially Buttons and set the visibility for Start(false) and New Game(true) buttons
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
        /// This function will countdown 1 in every sec
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

            //Write the actually tiem until start in the TextBlock
            MainWindow.TextBlockStartCountDown.Text = timeUntylStart;

            if (StartTime <= TimeSpan.FromSeconds(0))
            {//If the time until start is 0 then stop countdown and start the game
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
            var number = random.Next(0,2);
            TurnDownCard();

            //50% chance to pick old Card (number=0), 50% to pick a new Card (number=1)
            if (number <= 0 & CardBefore != FontAwesomeIcon.None)
            {//will pick the OLD Card
                MainWindow.CardPlaceRight.Icon = CardBefore;
            }
            else
            {//will pick a NEW Card
                bool isOldCard = true;
                while (isOldCard)
                {//Check if the picked new card is the old card, when yes then will pick a new one
                    //number will pick a Card from the Deck from 0 to 4, 4 is exclusive
                    number = random.Next(0, CardDeck.NrOfCardsInPlay);
                    if (MainWindow.CardPlaceRight.Icon == CardDeck.Cards[number])
                    {//the new Card is the Old ard
                        isOldCard = true;
                    }
                    else
                    {//the New card is different from the Old card
                        isOldCard = false;
                        MainWindow.CardPlaceRight.Icon = CardDeck.Cards[number];
                    }
                    
                }
            }
            
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
            GameCounters.BestReactionTime = stopwatch.ElapsedMilliseconds.ToString(); //read the reactiontime
            MainWindow.CardPlaceLeft.Icon = FontAwesomeIcon.Check;                  
            MainWindow.CardPlaceLeft.Foreground = Brushes.Green;
            HidingAnswer();
            GameCounters.CountStreak(true);
            GameCounters.CountPoints();
            ShowGameCounters();
            stopwatch.Restart(); //restart stopwatch for reactiontime
        }

        /// <summary>
        /// This will evaluate if the Answer is BAD
        /// </summary>
        private void TheAnswerIsBad()
        {
            MainWindow.CardPlaceLeft.Icon = FontAwesomeIcon.Times;
            MainWindow.CardPlaceLeft.Foreground = Brushes.Red;
            HidingAnswer();
            GameCounters.CountStreak(false);
            GameCounters.CountPoints();
            ShowGameCounters();
            stopwatch.Restart(); //restart stopwatch for reactiontime
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
        /// Game Over
        /// </summary>
        private void GameOver()
        {
            PendulumClock.Stop();
            DisableButtons();
            IsGame = false;
            TopScores= new HighScoreWindow(GameCounters);
        }
    }
}
