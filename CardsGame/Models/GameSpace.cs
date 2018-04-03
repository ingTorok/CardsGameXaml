using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
        /// Reached points
        /// </summary>
        private int TotalPoints = 0;

        /// <summary>
        /// Show the Reaction Time
        /// </summary>
        private int ReactionTime = 0;

        /// <summary>
        /// Countdowner
        /// </summary>
        DispatcherTimer PendulumClock;

        /// <summary>
        /// Gametime. Set to 45 sec
        /// </summary>
        private double GameTime = 5;

        /// <summary>
        /// Constructor, on creation get the MainWindow
        /// </summary>
        /// <param name="mainWindow">Window, which is showed on game progress</param>
        public GameSpace(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            SetNewGameCounters();
            EnableButtons();
            SetPendulumClock();
            CardDeck = new CardDeck();

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
            MainWindow.LabelCountDown.Content = $"Time remaining: {RemainedTime.ToString("mm\\:ss")}";
            MainWindow.LabelPoints.Content = $"Points {TotalPoints}";
            MainWindow.LabelReaction.Content = $"Reaction Time {ReactionTime}";
        }

        /// <summary>
        /// Enable Yes/No/Partially Buttons
        /// </summary>
        private void EnableButtons()
        {
            MainWindow.ButtonNo.IsEnabled = true;
            MainWindow.ButtonYes.IsEnabled = true;
            MainWindow.ButtonPartially.IsEnabled = true;
        }

        /// <summary>
        /// Disable Yes/No/Partially Buttons
        /// </summary>
        private void DisableButtons()
        {
            MainWindow.ButtonNo.IsEnabled = false;
            MainWindow.ButtonYes.IsEnabled = false;
            MainWindow.ButtonPartially.IsEnabled = false;
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
            ShowGameCounters();
            if (RemainedTime == TimeSpan.Zero)
            {
                GameOver();
            }
        }

        /// <summary>
        /// Game Over
        /// </summary>
        private void GameOver()
        {
            PendulumClock.Stop();
            DisableButtons();
        }
    }
}
