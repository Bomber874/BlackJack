using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public override string ToString()
        {
            return Rank + " " + "♥♦♣♠"[(int)Suit];
        }
    }
}
