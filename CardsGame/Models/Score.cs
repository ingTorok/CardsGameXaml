﻿namespace CardsGame.Models
{
    class Score
    {
        public string Name { get; set; }
        public long TotalPoints { get; set; }
        public string BestStreak { get; set; }
        public string BestReaction { get; set; }
        public string GameDate { get; set; }
    }
}