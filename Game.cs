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
        /// <param name="Deck">Номер колоды, куда была помещена карта</param>
        /// <param name="Card">Новая карта</param>
        /// <param name="FirstCards">true если карта выдана при первом круге сдачи карт</param>
        public delegate PlayerAction? PlayerTakeCardHandler(Player Player, byte Deck, Card Card, bool? FirstCards);
        public event PlayerTakeCardHandler? OnPlayerTakeCard;

        public delegate PlayerAction PlayerCationHandler(Player Player, byte deck);
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

        int AddPlayerCard(Player player, byte deck, Card card, bool? ForceCard)
        {
            player.AddCard(deck, card);
            OnPlayerTakeCard?.Invoke(player, deck, card, ForceCard);
            if (player.GetScore(deck) > 21) return 1;     // >21
            if (player.GetScore(deck) < 21) return -1;    //<21
            return 0;   //21
        }

        public void Start(List<Player> players)
        {
            if (players.Count <= 0) return;
            if (players.Count > 5) return;

            //Cards = Deck.Generate(true);
            Cards = new Queue<Card>(new Card[] { new Card() { Rank=(Rank)4,Suit=(Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 } });
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
                        OnPlayerTakeCard?.Invoke(p, 0, card, true);
                    }
                    else
                    {
                        if (AddPlayerCard(p, 0, Cards.Dequeue(), true) == 0)
                        {
                            Console.WriteLine("Не Тут обработать блэкджек");
                        }
                    }
                    
                }
                CroupierTakeCard();
            }


            foreach (Player p in _Players)
            {
                if (p.GetScore(0) != 21)
                {
                    PlayerActionsLoop(p, 0, null);
                }
                else
                {
                    Console.WriteLine("Вот тут обработать блекджек");
                }
            }
        }
        // TODO: Добавить события колоды: >21, 21 и тд
        void PlayerActionsLoop(Player player, byte deck, Card? card)
        {
            if (deck >= player.Decks.Count)
                return;
            switch (GetPlayerAction?.Invoke(player, deck))
            {
                case PlayerAction.Hit:
                    switch(AddPlayerCard(player, deck, Cards.Dequeue(), false))
                    {
                        case (-1)://<21
                            PlayerActionsLoop(player, deck, null);
                            break;
                        case (0)://21
                            Console.WriteLine("Колода:"+deck+", игрока:"+player.Name+" - набрала 21 очко");
                            PlayerActionsLoop(player, (byte)(deck + 1), null);
                            break;
                        case (1)://>21
                            Console.WriteLine("Колода:" + deck + ", игрока:" + player.Name + " - перебор");
                            PlayerActionsLoop(player, (byte)(deck + 1), null);
                            break;
                    }
                    break;
                case PlayerAction.Stand:
                    Console.WriteLine("Итоговые очки колоды:" + deck + "-" + player.GetScore(deck));
                    PlayerActionsLoop(player, (byte)(deck + 1), null);
                    break;
                case PlayerAction.Double:
                    AddPlayerCard(player, deck, Cards.Dequeue(), false);   // После удвоения ставки выдаётся карта, без возможности ходить ещё
                    PlayerActionsLoop(player, (byte)(deck + 1), null);
                    break;
                case PlayerAction.Split:
                    if (!player.CanSplit(deck))
                    {
                        PlayerActionsLoop(player, deck, null);
                        return;
                    }
                    player.AddDeck(player.RemoveLastCard(deck));
                    AddPlayerCard(player, deck, Cards.Dequeue(), false);
                    AddPlayerCard(player, (byte)(deck + 1), Cards.Dequeue(), false);
                    PlayerActionsLoop(player, deck, null);
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
