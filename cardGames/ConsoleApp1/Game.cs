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
        private int Number { get; set; }

        public Game(int nb)
        {
            Number = nb;
            for (int i = 0; i < NbCard; ++i)
            {
                deck.Add((Card)Math.Floor((decimal)i/4));
            }
        }

        private void DistribCards()
        {
            foreach (Client client in players)
            {
                //client.Write("Distributing the cards");
            }
            Random rand = new Random();
            while (deck.Count != 0)
            {
                int nbCard = rand.Next(deck.Count);
                players[deck.Count%maxNbPlayers].AddCard(deck[nbCard]);
                deck.RemoveAt(nbCard);
            }
        }

        private void BeginGame()
        {
            DistribCards();
        }

        public void AddClient(Connection connection)
        {
            players.Add(new Client(connection));
            //connection.SendObject("Added to game n°" + nb);
            if (IsFull())
            {
                foreach (Client client in players)
                {
                    //client.Write("The party will begin");
                }
                BeginGame();
            }
        }

        public bool IsFull()
        {
            return players.Count == maxNbPlayers;
        }
    }
}
