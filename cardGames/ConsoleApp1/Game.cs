using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;
using Protocol;

namespace cardGame_Server
{
    class Game
    {
        private static int maxNbPlayers = 2;
        private Deck deck { get; set; }
        private List<Client> players = new List<Client>();
        private List<Cards> well = new List<Cards>();

        private int Number { get; set; }
        private bool Running { get; set; }
        private bool Playing { get; set; }

        public Game(int nb)
        {
            Running = false;
            Playing = false;
            Number = nb;
            deck = new Deck();
        }

        private void DistribCards()
        {
            Console.WriteLine("Distributing the cards");
            Random rand = new Random();
            int idx = 0;
            Cards card;
            while ((card = deck.GetNextCard()) != Cards.None)
            {
                players[idx % maxNbPlayers].AddCard(card);
                ++idx;
            }
        }

        public void BeginGame()
        {
            Console.WriteLine("The game is begining");
            if (Running)
                return;
            Running = true;
            DistribCards();
            Console.WriteLine("Waiting now for all players to be ready");
        }

        public void AddClient(Client cl)
        {
            Console.WriteLine("Adding player");
            players.Add(cl);
            cl.Write("Added to game n°" + Number + ". Waiting for a challenger.");
            if (IsFull())
                foreach (Client client in players)
                {
                   client.Write("The hobby is now full. The game will begin soon...");
                }
        }

        public bool RemoveClient(Connection connection)
        {
            foreach (Client cl in players)
            {
                if (cl.IsEqual(connection))
                {
                    players.Remove(cl);
                    if (Running)
                        ResetGame(true);
                    return true;
                }
            }
            return false;
        }

        private void ResetGame(bool quit)
        {
            foreach (Client cl in players)
            {
                if (quit)
                    cl.Write("A player left the game. Please wait until another player comes...");
                cl.setReadyState(false);
                cl.TossHand();
            }
            deck = new Deck();
            Running = false;
            Playing = false;
            Console.WriteLine("Game reset");
        }

        public bool IsFull()
        {
            return maxNbPlayers == 0 ? false : players.Count == maxNbPlayers;
        }

        public int nbPlayers()
        {
            return players.Count;
        }

        public bool IsRunning()
        {
            return Running;
        }

        public bool IsPlaying()
        {
            return Playing;
        }

        public void PrepareGame()
        {
            foreach (Client client in players)
            {
                if (!client.IsReady())
                    return;
            }
            foreach (Client client in players)
            {
                client.SendCmd(Cmd.Ready);
                client.setReadyState(false);
            }
            Playing = true;
            Console.WriteLine("Game prepared.");
        }

        public void DoTurn()
        {
            foreach (Client client in players)
            {
                if (!client.IsReady())
                    return;
            }
            Console.WriteLine("Doing turn");
            PrepareTurn();
            foreach (Client client in players)
            {
                client.setReadyState(false);
            }
        }

        public void PrepareTurn()
        {
            if (FillWell())
                return;
            CheckWinTurnCondition();
        }

        public bool FillWell()
        {
            Console.WriteLine("Fill well");
            foreach (Client client in players)
            {
                Cards card = client.RemoveCard();
                if (card == Cards.None)
                {
                    CheckWinGameCondition();
                    return true;
                }
                well.Add(card);
            }
            return false;
        }

        public void CheckWinTurnCondition()
        {
            Console.WriteLine("Check win turn");
            Cards higher = Cards.None;
            List<int> winners = new List<int>();
            for (int i = well.Count - maxNbPlayers; i < well.Count - 1; ++i)
            {
                if (higher == Cards.None)
                {
                    higher = well[i];
                    winners.Add(i);
                }
                else if (well[i] >= higher)
                {
                    if (well[i] > higher)
                    {
                        higher = well[i];
                        winners.RemoveRange(0, winners.Count);
                    }
                    winners.Add(i);
                }
            }
            if (winners.Count > 1)
            {
                FillWell();
                foreach (int i in winners)
                {
                    players[winners[i]].SendCmd(Cmd.Draw);
                }
            }
            else
                DisplayVictory(players[winners[0]]);
            foreach (Client client in players)
            {
                if (client.NbCards() == 0)
                {
                    ResetGame(false);
                    break;
                }
            }
        }

        public void CheckWinGameCondition()
        {
            Console.WriteLine("Check win game");
            foreach (Client cl in players)
            {
                if (cl.NbCards() == 0)
                    cl.SendCmd(Cmd.Lose);
                else
                    cl.SendCmd(Cmd.Win);
            }
            ResetGame(false);
        }

        public void RestartGame()
        {
            ResetGame(false);
        }

        public void DisplayVictory(Client winner)
        {
            foreach (Client cl in players)
            {
                if (cl == winner)
                {
                    while (well.Count != 0)
                    {
                        winner.AddCard(well[0]);
                        well.RemoveAt(0);
                    }
                    cl.SendCmd(Cmd.Win);
                }
                else
                    cl.SendCmd(Cmd.Lose);
            }
        }
    }
}
