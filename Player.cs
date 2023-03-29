using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Cards = new List<Card>();
        public Player(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Очищает карты игрока
        /// </summary>
        public void ClearCards()
        {
            Cards.Clear();
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public List<Card> GetSortedCards()
        {
            List<Card> SortedCards = Cards;
            CardsSorter sorter = new CardsSorter();
            SortedCards.Sort(sorter);
            return SortedCards;
        }

        public byte GetScore()
        {
            byte score = 0;
            byte i = 0;
            byte aceCount = 0;
            foreach (Card card in GetSortedCards())
            {
                if ((int)card.Rank < 11)    // Карты с 2 до 10, считаем по их стоимости
                {
                    score += (byte)card.Rank;
                    i++;
                    continue;
                }
                else if((int)card.Rank < 14)    // Карты с рубашками, считаем за 10
                {
                    score += 10;
                    i++;
                    continue;
                }
                if (score + 11 <= 21)   // Первый туз, считаем за 11
                {
                    score += 11;
                    i++;
                    aceCount++;
                    continue;
                }
                else    // Последующие тузы, считаем за 1
                {
                    score += 1;
                    continue;
                }

            }
            return score;
        }

        public string GetCardsString()
        {
            string output = "|";
            foreach (Card card in Cards)
            {
                output = output + card.ToString() + "|"; 
            }
            return output;
        }

    }
}
