using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsGame.Models
{
    class CardDeck
    {
        /// <summary>
        /// Randoms for the Icons;
        /// </summary>
        private Random RandomIcon = new Random();
        
        /// <summary>
        /// List with Icons for Cards
        /// </summary>
        public List<FontAwesomeIcon> Cards = new List<FontAwesomeIcon>();

        /// <summary>
        /// 
        /// </summary>
        public CardDeck()
        {
            FillCardsListWithIcons();
            PickCards();
        }

        /// <summary>
        /// Number of card type in game
        /// </summary>
        public static int NrOfCardsInPlay { get; private set; } = 4;

        /// <summary>
        /// Number of cards in the deck
        /// </summary>
        public static int NrOfCardInDeck { get; private set; } = 7;


        /// <summary>
        /// Fill the Cards Array with Icons
        /// </summary>
        private void FillCardsListWithIcons()
        {
            Cards.Add(FontAwesomeIcon.Car);
            Cards.Add(FontAwesomeIcon.Bolt);
            Cards.Add(FontAwesomeIcon.Book);
            Cards.Add(FontAwesomeIcon.Circle);
            Cards.Add(FontAwesomeIcon.Cloud);
            Cards.Add(FontAwesomeIcon.LemonOutline);
            Cards.Add(FontAwesomeIcon.Star);
            Cards.Add(FontAwesomeIcon.Tree);
        }

        /// <summary>
        /// Pick the cards for the game randomly, without duplicate it
        /// </summary>
        private void PickCards()
        {
            while (Cards.Count != NrOfCardsInPlay)
            {
                Random random = new Random();
                var number = random.Next(0, Cards.Count);
                Cards.Remove(Cards[number]);
            }
        }
        
    }
}
