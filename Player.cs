using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BlackJack
{
    public class Player
    {
        public string Name { get; set; }
        //public List<Card> Cards = new List<Card>();
        public List<Card[]> Decks = new List<Card[]>();
        int _Balance;
        public int Balance { get => _Balance; }
        List<int> _Bet;
        public List<int> Bet { get => _Bet; }
        public void GameEnds(byte deck, Outcome outcome)
        {
            switch (outcome)
            {
                case Outcome.Lose:
                    _Bet[deck] = 0;
                    break;
                case Outcome.Draw:
                    _Balance += _Bet[deck];
                    _Bet[deck] = 0;
                    break;
                case Outcome.Win:
                    _Balance += _Bet[deck]*2;
                    _Bet[deck] = 0;
                    break;
                case Outcome.BlackJack:
                    _Balance += Convert.ToInt32(Math.Floor(_Bet[deck] * 1.5));
                    _Bet[deck] = 0;
                    break;
            }
        }
        public Player(string name)
        {
            Name = name;
            _Balance = 1000;
            _Bet = new List<int>();
        }
        public bool AddBet(byte Deck, int bet)
        {
            if (_Bet.Count <= Deck)
            {
                _Bet.Add(bet);
                _Balance -= bet;
                return true;    // дада
            }
            _Bet[Deck] += bet;
            _Balance -= bet;
            return true;    // дада
        }
        public bool CanDouble(byte deck)
        {
            return Balance - Bet[deck] >= 0;
        }
        /// <summary>
        /// Очищает карты игрока
        /// </summary>
        public void ClearCards()
        {
            Decks.Clear();
        }
        public void AddDeck(byte CurDeck, Card card)
        {
            if (CurDeck == Decks.Count)
            {
                Decks.Insert(CurDeck, new Card[] { card });
            }
            else
            {
                Decks.Insert(CurDeck + 1, new Card[] { card });
            }
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

        public Card[] GetSortedCards(byte Deck)
        {
            CardsSorter sorter = new CardsSorter();
            Card[] SortedCards = Decks[Deck];
            for (byte i = 0; i<SortedCards.Length; i++)
            {
                Array.Sort(SortedCards, sorter);
            }
            //SortedCards.Sort(sorter);
            return SortedCards;
        }

        public byte GetScore(byte Deck)
        {
            byte score = 0;
            byte aceCount = 0;

             foreach (Card card in GetSortedCards(Deck))
             {
                 if ((int)card.Rank < 11)    // Карты с 2 до 10, считаем по их стоимости
                 {
                     score += (byte)card.Rank;
                     continue;
                 }
                 else if ((int)card.Rank < 14)    // Карты с рубашками, считаем за 10
                 {
                     score += 10;
                     continue;
                 }
                 if (score + 11 <= 21)   // Первый туз, считаем за 11
                 {
                     score += 11;
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
        public bool CanSplit(byte deck)
        {
            if (Decks[deck].Length > 2) return false;   // Колоду возможно сплитить только на двух одинаковых картах
            if (!CanDouble(deck)) return false; // Сплит подразумевает и увеличение общей ставки
            if (Decks[deck][0].Rank == Decks[deck][1].Rank) // Если карды совпадают по значению
                return true;
            // Если карты являются картинками
            if ((Decks[deck][0].Rank < (Rank)15 & Decks[deck][0].Rank > (Rank)9) & (Decks[deck][1].Rank < (Rank)15 & Decks[deck][1].Rank > (Rank)9))
            {
                return true;
            }
            return false;
        }

        public string GetCardsString(byte Deck)
        {
            string output = "|";

            foreach (Card card in Decks[Deck])
            {
                output += card.ToString() + "|";
            }
        return output;
        }

    }
}
