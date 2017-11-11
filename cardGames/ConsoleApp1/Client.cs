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
   
        public Client(Connection connection)
        {
            Connect = connection;
            Name = Connect.ToString();
            Ready = false;
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public int NbCards()
        {
            return hand.Count;
        }

        public void Write(string msg)
        {
            //Connect.SendObject(msg);
        }

        public void switchReadyState()
        {
            Ready = !Ready;
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
