using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace cardGame_Server
{
    enum Card : byte { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

    class Game
    {
        private static int NbCard = 52;

        private List<Client> players = new List<Client>();
        private int maxNbPlayers = 2;
        private int nbTurn { get; set; }
        private List<Card> deck = new List<Card>();

        public Game()
        {
            for (int i = 0; i < NbCard; ++i)
            {
                deck.Insert(0, (Card)Math.Floor((decimal)i/4));
            }
        }

        public void distribCards()
        {
            
        }

        public void addClient(Connection connection)
        {
            
        }

        public void isEmpty
    }
}
