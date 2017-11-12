using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

namespace cardGame_Server
{
    class Deck
    {
        private List<int> deck = new List<int>();
        private int availablecards;
        private Random rnd;

        public Deck()
        {
            availablecards = 52;
            for (int i = 0; i < 13; ++i)
                deck.Add(4);
            rnd = new Random();
        }

        public Cards GetNextCard()
        {
            if (deck.Count == 0)
                return Cards.None;
            int dest = rnd.Next(13);

            while (deck[dest] <= 0) { dest = rnd.Next(1, 13); }
            deck[dest]--;
            availablecards--;
            return ((Cards)dest);
        }
    }
}