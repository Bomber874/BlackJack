using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    static public class Deck
    {
        // Играть буду в одну колоду, так вероятность победы выше, нежели с бесконечной
        static public Queue<Card> Generate(bool shuffle)
        {
            if (shuffle)
            {
                Random random = new Random();
                Card[] cards = new Card[52];
                Queue<Card> queue = new Queue<Card>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        cards[i * 13 + j] = new Card { Rank = (Rank)j + 2, Suit = (Suit)i };
                    }
                }
                // Перемешиваем
                for (int i = 51; i >= 1; i--)
                {
                    int j = random.Next(i + 1);
                    // обменять значения data[j] и data[i]
                    var temp = cards[j];
                    cards[j] = cards[i];
                    cards[i] = temp;
                }
                foreach (Card card in cards)
                {
                    queue.Enqueue(card);
                }
                return queue;
            }
            else
            {
                Random random = new Random();
                Card[] cards = new Card[52];
                Queue<Card> queue = new Queue<Card>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        queue.Enqueue(new Card { Rank = (Rank)j + 2, Suit = (Suit)i });
                    }
                }
                return queue;
            }
        }
        //static public Card[] Generate(bool shuffle)
        //{
        //    if (shuffle)
        //    {
        //        Random random = new Random();
        //        Card[] cards = new Card[52];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 13; j++)
        //            {
        //                //(Rank)j+2 - Для удобства, начало перечисления с двойки
        //                cards[i * 13 + j] = new Card { Rank = (Rank)j + 2, Suit = (Suit)i };
        //            }
        //        }
        //        // Перемешиваем
        //        for (int i = 51; i >= 1; i--)
        //        {
        //            int j = random.Next(i + 1);
        //            // обменять значения data[j] и data[i]
        //            var temp = cards[j];
        //            cards[j] = cards[i];
        //            cards[i] = temp;
        //        }
        //        return cards;
        //    }
        //    else
        //    {
        //        Card[] cards = new Card[52];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 13; j++)
        //            {
        //                //(Rank)j+2 - Для удобства, начало перечисления с двойки
        //                cards[i * 13 + j] = new Card { Rank = (Rank)j + 2, Suit = (Suit)i };
        //            }
        //        }
        //        return cards;
        //    }
        //}
    }
}
