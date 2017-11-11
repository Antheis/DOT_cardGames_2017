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
        private int maxNbPlayers = 2;
        private int nbTurn { get; set; }

        private List<Client> players = new List<Client>();
        private List<Card> deck = new List<Card>();

        private int Number { get; set; }
        private bool Running { get; set; }

        public Game(int nb)
        {
            Running = false;
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

        public void BeginGame()
        {
            if (Running)
                return;
            Running = true;
            foreach (Client client in players)
            {
                //client.Write("The party will begin");
            }
            DistribCards();
        }

        public void AddClient(Connection connection)
        {
            players.Add(new Client(connection));
            //connection.SendObject("Added to game n°" + nb + ". Waiting for a challenger.");
        }

        public bool IsFull()
        {
            return players.Count == maxNbPlayers;
        }

        public int nbPlayers()
        {
            return players.Count;
        }

        public bool IsRunning()
        {
            return Running;
        }

        public void PrepareTurn()
        {
            foreach (Client client in players)
            {
                if (!client.IsReady())
                    return;
            }
        }
    }
}
