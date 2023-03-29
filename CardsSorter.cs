using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    internal class CardsSorter : IComparer<Card>
    {
        public int Compare(Card _a, Card _b)
        {
            Rank a = _a.Rank;
            Rank b = _b.Rank;
            if ((int)a < (int)b) return -1;
            if ((int)a > (int)b) return 1;
            return 0;
        }

    }
}
