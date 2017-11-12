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
            int idx = 0;
            Cards card;
            while ((card = deck.GetNextCard()) != Cards.None)
            {
                Console.WriteLine(card + " goes to " + idx%maxNbPlayers);
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
                    ExitGame();
                    return true;
                }
                well.Add(card);
                client.SetCardDrawn(card);
            }
            return false;
        }

        public void CheckWinTurnCondition()
        {
            Console.WriteLine("Check win turn");
            Console.WriteLine(players[0].GetCardDrawn() + " Vs " + players[1].GetCardDrawn());
            if (players[0].GetCardDrawn() > players[1].GetCardDrawn())
                DisplayVictory(players[0], players[1]);
            else if (players[0].GetCardDrawn() < players[1].GetCardDrawn())
                DisplayVictory(players[1], players[0]);
            else
            {
                players[0].SendCmd(Cmd.Draw);
                players[1].SendCmd(Cmd.Draw);
            }
        }

        public void DisplayVictory(Client winner, Client loser)
        {
            winner.SendCmd(Cmd.Win, new List<Cards>(new Cards[] {winner.GetCardDrawn(), loser.GetCardDrawn()}));
            loser.SendCmd(Cmd.Lose, new List<Cards>(new Cards[] { loser.GetCardDrawn(), winner.GetCardDrawn() }));
        }

        public void ExitGame()
        {
            Console.WriteLine("exiting game");
            foreach (Client cl in players)
            {
                cl.setGame(-1);
            }
            players = new List<Client>();
        }
    }
}
