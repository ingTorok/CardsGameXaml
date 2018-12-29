using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsGame.Models
{
    public class GameCounters
    {
        /// <summary>
        /// List of good or bad answers
        /// </summary>
        public string Streak { get; private set; } = "       ";

        /// <summary>
        /// Variable to count Points multiplier
        /// </summary>
        public int StreakMultiplier { get; private set; } = 0;

        /// <summary>
        /// Reached points
        /// </summary>
        public long TotalPoints { get; private set; } = 0;

        /// <summary>
        /// Variable to hold the best streak
        /// </summary>
        public int BestStreak { get; private set; } = 0;

        /// <summary>
        /// Variable to hold the worst streak
        /// </summary>
        public int WorstStreak { get; private set; } = 0;

        /// <summary>
        /// Variable to hold the best reaktion time
        /// </summary>
        public string BestReactionTime { get;  set; }

        /// <summary>
        /// Counting the Streak for good and bad answers
        /// </summary>
        /// <param name="v"></param>
        public void CountStreak(bool v)
        {
            if (v)
            {
                Streak += "1";

                if (StreakMultiplier <= 0)
                {
                    StreakMultiplier = 0;
                }

                StreakMultiplier++;

                if (BestStreak < StreakMultiplier)
                {
                    BestStreak = StreakMultiplier;
                }
            }
            else
            {
                Streak += "0";

                if (StreakMultiplier >= 0)
                {
                    StreakMultiplier = 0;
                }

                StreakMultiplier--;

                if (WorstStreak > StreakMultiplier)
                {
                    WorstStreak = StreakMultiplier;
                }
            }

        }

        /// <summary>
        /// Method to count Points
        /// </summary>
        public void CountPoints()
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

    }
}
