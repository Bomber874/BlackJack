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
        public delegate void CroupierShowsCardHandler();
        public event CroupierShowsCardHandler? OnCroupierShowsCard;
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

        public delegate int PlayerBetHandler(Player player, byte deck);
        public event PlayerBetHandler? GetPlayerBet;

        public delegate void PlayerDroppedFromGameHandler(Player player, string reason);
        public event PlayerDroppedFromGameHandler? PlayerDropped;

        public delegate void OutcomeHandler(Player player, byte deck, Outcome outcome);
        public event OutcomeHandler? OnOutcome;

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

        List<Card> GetSortedCards()
        {
            CardsSorter sorter = new CardsSorter();
            List<Card> SortedCards = Croupier;
            SortedCards.Sort(sorter);
            return SortedCards;
        }

        public byte GetScore()
        {
            byte score = 0;
            byte aceCount = 0;

            foreach (Card card in GetSortedCards())
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

        byte CroupierGetCards()
        {
            byte Score = GetScore();
            if (Score > 16)
            {
                return Score;
            }
            CroupierTakeCard(false);
            return CroupierGetCards();
        }

        public void Start(List<Player> players)
        {
            if (players.Count <= 0) return;
            if (players.Count > 5) return;

            Cards = Deck.Generate(true);
            //Cards = new Queue<Card>(new Card[] { new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 }, new Card() { Rank = (Rank)4, Suit = (Suit)1 } });
            _Players.Clear();

            foreach(Player p in players)
            {
                p.ClearCards();
                if (p.Balance < 50)
                {
                    PlayerDropped?.Invoke(p, "У игрока не хватает на минимальную ставку");
                    continue;
                }
                _Players.Add(p);
                p.AddBet(0, PlayerBet(p, 0));

            }

            for (byte i = 0; i < 2; i++)    // Первая сдача
            {
                foreach (Player p in players)
                {
                    if (i == 0)
                    {
                        Card card = Cards.Dequeue();
                        p.AddDeck(0, card);
                        OnPlayerTakeCard?.Invoke(p, 0, card, true);
                    }
                    else
                    {
                        if (AddPlayerCard(p, 0, Cards.Dequeue(), true) == 0)
                        {
                            OnOutcome?.Invoke(p, 0, Outcome.BlackJack);
                            //BlackJack
                            //Console.WriteLine("Не Тут обработать блэкджек");
                        }
                    }
                }
                CroupierTakeCard(i>0);
            }


            foreach (Player p in _Players)
            {
                if (p.GetScore(0) != 21)
                {
                    PlayerActionsLoop(p, 0, null);
                }
                else
                {
                    //BlackJack
                }
            }
            OnCroupierShowsCard?.Invoke();
            byte CroupierScore = CroupierGetCards();
            foreach(Player p in _Players)
            {
                for (byte i = 0; i < p.Decks.Count; i++)
                {
                    if (p.Bet[i] == 0) continue;    // Если на этом этапе ставка == 0, значит у игрока блекджек(уже выплаченный)
                    byte Score = p.GetScore(i);
                    if (Score > 20) continue;   // Победы и проигрыши уже просчитаны
                    if (Score == CroupierScore)
                    {
                        p.GameEnds(i, Outcome.Draw);
                        OnOutcome?.Invoke(p, i, Outcome.Draw);
                    }
                    if (Score > CroupierScore)
                    {
                        p.GameEnds(i, Outcome.Win);
                        OnOutcome?.Invoke(p, i, Outcome.Win);
                    }
                    if (Score < CroupierScore)
                    {
                        p.GameEnds(i, Outcome.Lose);
                        OnOutcome?.Invoke(p, i, Outcome.Lose);
                    }
                }
            }

        }

        int PlayerBet(Player player, byte deck)
        {
            int Bet = (GetPlayerBet?.Invoke(player, deck)).GetValueOrDefault(50);
            Bet = Bet < 0 ? 50 : Bet > player.Balance ? Convert.ToInt32(player.Balance) : Bet;
            return Bet;
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
                            player.GameEnds(deck, Outcome.Win);
                            OnOutcome?.Invoke(player, deck, Outcome.Win);
                            PlayerActionsLoop(player, (byte)(deck + 1), null);
                            break;
                        case (1)://>21
                            player.GameEnds(deck, Outcome.Lose);
                            OnOutcome?.Invoke(player, deck, Outcome.Lose);
                            PlayerActionsLoop(player, (byte)(deck + 1), null);
                            break;
                    }
                    break;
                case PlayerAction.Stand:
                    PlayerActionsLoop(player, (byte)(deck + 1), null);
                    break;
                case PlayerAction.Double:
                    if (!player.CanDouble(deck))
                    {
                        PlayerActionsLoop(player, deck, null);
                        return;
                    }
                    player.AddBet(deck, player.Bet[deck]);
                    switch (AddPlayerCard(player, deck, Cards.Dequeue(), false))
                    {
                        case (-1)://<21
                            break;
                        case (0)://21
                            player.GameEnds(deck, Outcome.Win);
                            OnOutcome?.Invoke(player, deck, Outcome.DoubleWin);
                            break;
                        case (1)://>21
                            player.GameEnds(deck, Outcome.Lose);
                            OnOutcome?.Invoke(player, deck, Outcome.DoubleLose);
                            break;
                    }
                    //player.AddBet(deck, player.Bet[deck]);
                    PlayerActionsLoop(player, (byte)(deck + 1), null);
                    break;
                case PlayerAction.Split:
                    if (!player.CanSplit(deck))
                    {
                        PlayerActionsLoop(player, deck, null);
                        return;
                    }
                    player.AddDeck(deck, player.RemoveLastCard(deck));
                    player.AddBet((byte)(deck+1), player.Bet[deck]);
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
        void CroupierTakeCard(bool HoleCard)
        {
            Card card = Cards.Dequeue();
            _Croupier.Add(card);
            OnCroupierTakeCard?.Invoke(HoleCard, HoleCard ? null : card); // Остальные карты кладутся рубашкой к верху
        }
    }
}
