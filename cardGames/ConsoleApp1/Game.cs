using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;
using Protocol;

namespace cardGame_Server
{
    class Game
    {
        private static int NbCard = 52;
        private static int maxNbPlayers = 2;
        private int nbTurn { get; set; }

        private List<Client> players = new List<Client>();
        private List<Card> deck = new List<Card>();

        private int Number { get; set; }
        private bool Running { get; set; }

        public Game(int nb)
        {
            Running = false;
            Number = nb;
            ShuffleDeck();
        }

        public void ShuffleDeck()
        {
            for (int i = 0; i < NbCard; ++i)
            {
                deck.Add((Card)Math.Floor((decimal)i / 4));
            }
        }

        private void DistribCards()
        {
            foreach (Client client in players)
            {
                //client.Write("Distributing the cards");
            }
            Console.WriteLine("Distributing the cards...");
            Random rand = new Random();
            while (deck.Count != 0)
            {
                int nbCard = rand.Next(deck.Count);
                players[deck.Count % maxNbPlayers].AddCard(deck[nbCard]);
                Console.WriteLine((int)deck[nbCard] + "goes to player " + deck.Count % maxNbPlayers);
                deck.RemoveAt(nbCard);
            }
        }

        public void BeginGame()
        {
            if (Running)
                return;
            Running = true;
            Console.WriteLine("The Game has begun");
            foreach (Client client in players)
            {
                //client.Write("The party will begin");
            }
            DistribCards();
            Console.WriteLine("Waiting now for all players to be ready");
        }

        public void AddClient(Client cl)
        {
            Console.WriteLine("Adding player");
            players.Add(cl);
            //connection.SendObject("Added to game n°" + nb + ". Waiting for a challenger.");
        }

        public bool RemoveClient(Connection connection)
        {
            foreach (Client cl in players)
            {
                if (cl.IsEqual(connection))
                {
                    players.Remove(cl);
                    if (Running)
                        ResetGame();
                    return true;
                }
            }
            return false;
        }

        private void ResetGame()
        {
            foreach (Client cl in players)
            {
                cl.Write("A player left the game. Please wait until another player comes...");
                cl.setReadyState(false);
                cl.TossHand();
            }
            ShuffleDeck();
            Running = false;
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
