using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Player
    {
        public string Name { get; set; }
        List<Card> Cards = new List<Card>();
        public Player(string name)
        {
            Name = name;
        }
        public void Restart()
        {
            Cards.Clear();
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public int GetScore()
        {
            return Cards.Count;
        }
        public string GetCards()
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
