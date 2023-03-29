using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Game
    {
        /// <summary>
        /// Вызывается при выдаче крупье карты.
        /// </summary>
        /// <param name="HoleCard">Взята ли карта в закрытую(hole card)</param>
        /// <param name="Card">Если карта взята в открытую, содержит карту, иначе - NULL</param>
        public delegate void CroupierTakeCardHandler(bool HoleCard, Card? Card);
        public event CroupierTakeCardHandler? OnCroupierTakeCard;
        /// <summary>
        /// Вызывается при выдаче игроку карты
        /// </summary>
        /// <param name="Player">Игрок, взявший карту</param>
        /// <param name="Card">Новая карта</param>
        /// <param name="Score">Счёт игрока, с учётом новой карты</param>
        public delegate void PlayerTakeCardHandler(Player Player, Card Card, byte Score);
        public event PlayerTakeCardHandler? OnPlayerTakeCard;

        //public delegate 

        Queue<Card> Cards = Deck.Generate(true);

        public List<Player> Players { get => _Players; }
        public List<Card> Croupier { get => _Croupier; }
        public byte CurPlayerIndex { get => _CurPlayerIndex; }
        public Player CurPlayer { get => _CurPlayer; }

        List<Player> _Players = new List<Player>();
        List<Card> _Croupier = new List<Card>();
        byte _CurPlayerIndex = 0;
        Player _CurPlayer = new Player("");
        // Реализовать очередь игроков


        public void Start(List<Player> players)
        {
            if (players.Count <= 0) return;
            if (players.Count > 5) return;

            Cards = Deck.Generate(true);
            _Players.Clear();
            foreach (Player p in players)
            {
                p.ClearCards();
                _Players.Add(p);
                for(byte i = 0; i<2; i++)
                {
                    Card card = Cards.Dequeue();
                    p.AddCard(card);
                    OnPlayerTakeCard?.Invoke(p, card, p.GetScore());
                }
                
            }

            CroupierTakeCard();
            CroupierTakeCard();

            _CurPlayerIndex = 0;


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
        void CroupierTakeCard()
        {
            _Croupier.Add(Cards.Dequeue());
            if (Croupier.Count == 1)
            {
                OnCroupierTakeCard?.Invoke(false, Croupier[0]); // Первая карта кладётся на стол в открытую
            }
            else
            {
                OnCroupierTakeCard?.Invoke(true, null); // Остальные карты кладутся рубашкой к верху
            }
        }
    }
}
