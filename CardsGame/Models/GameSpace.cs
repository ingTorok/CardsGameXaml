using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Constructor, on creation get the MainWindow
        /// </summary>
        /// <param name="mainWindow">Window, which is showed on game progress</param>
        public GameSpace(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            SetNewGameCounters();

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
            RemainedTime = TimeSpan.FromSeconds(45);
            CardDeck = new CardDeck();
            ShowGameCounters();
        }

        /// <summary>
        /// show and refresh the gama counters
        /// </summary>
        private void ShowGameCounters()
        {
            MainWindow.LabelCountDown.Content = $"Time remain: {RemainedTime.ToString("mm//:ss")}";
            MainWindow.LabelPoints.Content = $"Points {TotalPoints}";
            MainWindow.LabelReaction.Content = $"Reaction Time {ReactionTime}";
        }
    }
}
