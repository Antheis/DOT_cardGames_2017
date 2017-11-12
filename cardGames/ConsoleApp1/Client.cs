using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;
using Protocol;

namespace cardGame_Server
{
    class Client
    {
        private List<Card> hand = new List<Card>();
        private Card CardDrawn { get; set; }
        private Connection Connect { get; set; }
        private int Id { get; set; }
        private bool Ready { get; set; }
        private int NbGameInto { get; set; }
   
        public Client(Connection connection)
        {
            Connect = connection;
            Ready = false;
            NbGameInto = -1;
            CardDrawn = Card.None;
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public Card RemoveCard()
        {
            if (hand.Count == 0)
                return Card.None;
            Card card = hand[0];
            hand.RemoveAt(0);
            return card;
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

        public void SetCardDrawn(Card card)
        {
            CardDrawn = card;
        }

        public Card GetCardDrawn()
        {
            return CardDrawn;
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
