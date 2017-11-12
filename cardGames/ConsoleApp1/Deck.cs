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
        private List<Cards> deck = new List<Cards>();
        private int availablecards;
        private Random rnd;

        public Deck()
        {
            availablecards = 52;
            for (int i = 0; i < availablecards; ++i)
                deck.Add((Cards)(i/4));
            rnd = new Random();
        }

        public Cards GetNextCard()
        {
            if (availablecards == 0)
                return Cards.None;
            int dest = rnd.Next(availablecards);
            Cards card = deck[dest];
            deck.RemoveAt(dest);
            availablecards--;
            return card;
        }
    }
}