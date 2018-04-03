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
        /// Array to keep the three cards which will be used for the actually game
        /// </summary>
        private int[] IconToUse = new int[4];
        
        /// <summary>
        /// Array with Icons for Cards
        /// </summary>
        private FontAwesomeIcon[] Cards = new FontAwesomeIcon[8];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListOfCards"></param>
        public CardDeck()
        {
            FillCardsArrayWithIcons();
            PickCards();
        }

        /// <summary>
        /// Number of card type in game
        /// </summary>
        public int NumberOfCardsInPlay { get; private set; } = 4;

        /// <summary>
        /// Number of cards in the deck
        /// </summary>
        public int NrOfCardInDeck { get; private set; } = 7;


        /// <summary>
        /// Fill the Cards Array with Icons
        /// </summary>
        private void FillCardsArrayWithIcons()
        {
            Cards[0] = FontAwesomeIcon.Car;
            Cards[1] = FontAwesomeIcon.Bolt;
            Cards[2] = FontAwesomeIcon.Book;
            Cards[3] = FontAwesomeIcon.Circle;
            Cards[4] = FontAwesomeIcon.Cloud;
            Cards[5] = FontAwesomeIcon.LemonOutline;
            Cards[6] = FontAwesomeIcon.Star;
            Cards[7] = FontAwesomeIcon.Tree;
        }

        /// <summary>
        /// Pick the cards for the game randomly, without duplicate it
        /// </summary>
        private void PickCards()
        {
            for (int i = 0; i < NumberOfCardsInPlay; i++)
            {
                bool check = true;

                while (check)
                {
                    var number = RandomIcon.Next(0, NrOfCardInDeck);
                    IconToUse[i] = number;

                    for (int j = 0; j <= i; j++)
                    {
                        if ( IconToUse[j] != number || i == j)
                        {//Check if Cards[Number] exist in the cards which we want to play
                            check = false;
                            
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                }
 
            }
        }

    }
}
