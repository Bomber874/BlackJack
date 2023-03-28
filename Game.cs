using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Game
    {
        Queue<Card> Cards = Deck.Generate(true);

        public List<Player> Players { get => _Players; }
        public List<Card> Croupier { get => _Croupier; }
        
        List<Player> _Players = new List<Player>();
        List<Card> _Croupier = new List<Card>();
        
        
        
        public void Start(List<Player> players)
        {
            if (players.Count <= 0) return;
            if (players.Count > 5) return;

            Cards = Deck.Generate(true);
            _Players.Clear();
            foreach (Player p in players)
            {
                p.Restart();
                p.AddCard(Cards.Dequeue());
                p.AddCard(Cards.Dequeue());
                _Players.Add(p);
            }

            _Croupier.Add(Cards.Dequeue());
            _Croupier.Add(Cards.Dequeue());

        }
        /// <summary>
        /// Вернёт игрока с заданным именем
        /// Если игрок не найден, вернёт экземпляр Player с именем ""
        /// </summary>
        /// <param name="name">Имя игрока</param>
        /// <returns>Player найденный игрок</returns>
        public Player GetPlayerByName(string name)
        {
            foreach (Player p in _Players)
            {
                if (p.Name== name) return p;
            }
            return new Player("");
        }
    }
}
