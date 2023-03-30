using System;
using System.Collections.Generic;
using System.Numerics;
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
        /// <param name="FirstCards">true если карта выдана при первом круге сдачи карт</param>
        public delegate PlayerAction? PlayerTakeCardHandler(Player Player, byte Deck, Card Card, byte Score, bool? FirstCards);
        public event PlayerTakeCardHandler? OnPlayerTakeCard;

        public delegate PlayerAction PlayerCationHandler(Player Player);
        public event PlayerCationHandler? GetPlayerAction;

        Queue<Card> Cards = Deck.Generate(true);

        public List<Player> Players { get => _Players; }
        public List<Card> Croupier { get => _Croupier; }
        public Player CurPlayer { get => _CurPlayer; }

        List<Player> _Players = new List<Player>();
        List<Card> _Croupier = new List<Card>();
        Player _CurPlayer = new Player("");
        // Реализовать очередь игроков
        // А никакая очередь не нужна

        void AddPlayerCard(Player player, byte deck, Card card, bool? ForceCard)
        {
            player.AddCard(deck, card);
            OnPlayerTakeCard?.Invoke(player, deck, card, player.GetScore()[deck], ForceCard);
        }

        public void Start(List<Player> players)
        {
            if (players.Count <= 0) return;
            if (players.Count > 5) return;

            Cards = Deck.Generate(true);
            _Players.Clear();

            for (byte i = 0; i < 2; i++)    // Первая сдача
            {
                foreach (Player p in players)
                {
                    if (i == 0)
                    {
                        p.ClearCards();
                        _Players.Add(p);
                        Card card = Cards.Dequeue();
                        p.AddDeck(card);
                        OnPlayerTakeCard?.Invoke(p, 0, card, p.GetScore()[0], true);
                    }
                    else
                    {
                        AddPlayerCard(p, 0, Cards.Dequeue(), true);
                    }
                    
                }
                CroupierTakeCard();
            }


            foreach (Player p in _Players)
            {
                PlayerActionsLoop(p, 0, null, p.CanSplit(0));

            }
        }

        void PlayerActionsLoop(Player player, byte deck, Card? card, bool CanSplit)
        {
            switch (GetPlayerAction?.Invoke(player))
            {
                case PlayerAction.Hit:
                    AddPlayerCard(player, deck, Cards.Dequeue(), false);
                    PlayerActionsLoop(player, deck, null, false);
                    break;
                case PlayerAction.Stand:
                    break;
                case PlayerAction.Double:
                    AddPlayerCard(player, deck, Cards.Dequeue(), true);   // После удвоения ставки выдаётся карта, без возможности ходить ещё
                    break;
                case PlayerAction.Split:
                    player.AddDeck(player.RemoveLastCard(deck));
                    break;
            }
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
