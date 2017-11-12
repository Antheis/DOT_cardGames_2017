using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace cardGame_Server
{
    class Client
    {
        private List<Card> hand = new List<Card>();
        private Connection Connect { get; set; }
        private string Name { get; set; }
        private int Id { get; set; }
        private bool Ready { get; set; }
        private int NbGameInto { get; set; }
   
        public Client(Connection connection)
        {
            Connect = connection;
            Name = Connect.ToString();
            Ready = false;
            NbGameInto = -1;
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public int NbCards()
        {
            return hand.Count;
        }

        public void setGame(int game)
        {
            NbGameInto = game;
        }

        public int Game()
        {
            return NbGameInto;
        }

        public void Write(string msg)
        {
            Connect.SendObject("Message", msg);
        }

        public void TossHand()
        {
            hand.RemoveRange(0, hand.Count);
        }

        public void setReadyState(bool status)
        {
            Ready = status;
        }

        public bool IsEqual(Connection connection)
        {
            return Connect == connection;
        }

        public bool IsReady()
        {
            return Ready;
        }
    }
}
