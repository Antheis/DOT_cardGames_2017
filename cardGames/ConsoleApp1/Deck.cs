using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cardGames_Server
{
    class Deck
    {
        public enum cards { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
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

        public cards GetNextCard()
        {
            int dest = rnd.Next(1, 13);

            while (deck[dest] <= 0) { dest = rnd.Next(1, 13); }
            deck[dest]--;
            availablecards--;
            return ((cards)dest);
        }
    }
}
