using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Player
    {
        public string Name { get; set; }
        //public List<Card> Cards = new List<Card>();
        public List<Card[]> Decks = new List<Card[]>();
        public Player(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Очищает карты игрока
        /// </summary>
        public void ClearCards()
        {
            Decks.Clear();
        }
        public void AddDeck(Card card)
        {
            Decks.Add(new Card[] {card});
        }
        public void AddCard(byte Deck, Card card)
        {
            Card[] OldCards = Decks[Deck];
            Card[] NewCards = new Card[OldCards.Length+1];
            for (byte i = 0; i < OldCards.Length; i++)
            {
                NewCards[i] = OldCards[i];
            }
            NewCards[OldCards.Length] = card;
            Decks[Deck] = NewCards;
        }

        public Card RemoveLastCard(byte Deck)
        {
            Card RemovedCard = Decks[Deck][Decks[Deck].Length - 1];
            Card[] NewCards = new Card[Decks[Deck].Length - 1];
            for (byte i = 0; i < Decks[Deck].Length-1; i++)
            {
                NewCards[i] = Decks[Deck][i];
            }
            Decks[Deck] = NewCards;
            return RemovedCard;
        }

        public List<Card[]> GetSortedCards(byte Deck)
        {
            CardsSorter sorter = new CardsSorter();
            List<Card[]> SortedCards = Decks;
            for (byte i = 0; i<SortedCards.Count; i++)
            {
                Array.Sort(SortedCards[i], sorter);
            }
            //SortedCards.Sort(sorter);
            return SortedCards;
        }

        public byte[] GetScore()
        {
            byte[] score = new byte[Decks.Count];
            byte aceCount = 0;
            for (byte i = 0; i < Decks.Count; i++)
            {
                foreach (Card[] cards in GetSortedCards(i))
                {
                    foreach (Card card in cards)
                    {
                        if ((int)card.Rank < 11)    // Карты с 2 до 10, считаем по их стоимости
                        {
                            score[i] += (byte)card.Rank;
                            continue;
                        }
                        else if ((int)card.Rank < 14)    // Карты с рубашками, считаем за 10
                        {
                            score[i] += 10;
                            continue;
                        }
                        if (score[i] + 11 <= 21)   // Первый туз, считаем за 11
                        {
                            score[i] += 11;
                            aceCount++;
                            continue;
                        }
                        else    // Последующие тузы, считаем за 1
                        {
                            score[i] += 1;
                            continue;
                        }
                    }
                }
            }
            return score;
        }
        public bool CanSplit(byte deck)
        {
            if (Decks[deck][0].Rank == Decks[deck][1].Rank) // Если карды совпадают по значению
                return true;
            // Если карты являются картинками
            if ((Decks[deck][0].Rank < (Rank)15 & Decks[deck][0].Rank > (Rank)10) & (Decks[deck][1].Rank < (Rank)15 & Decks[deck][1].Rank > (Rank)10))
            {
                return true;
            }
            return false;
        }

        public string[] GetCardsString()
        {
            string[] output = new string[Decks.Count];
            for (byte i = 0; i < Decks.Count; i++)
            {
                foreach (Card card in Decks[i])
                {
                    output[i] += card.ToString() + "|";
                }
            }
            return output;
        }

    }
}
