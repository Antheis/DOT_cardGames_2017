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
        private List<Cards> hand = new List<Cards>();
        private Cards CardDrawn { get; set; }
        private Connection Connect { get; set; }
        private int Id { get; set; }
        private bool Ready { get; set; }
        private int NbGameInto { get; set; }
   
        public Client(Connection connection)
        {
            Connect = connection;
            Ready = false;
            NbGameInto = -1;
            CardDrawn = Cards.None;
        }

        public void AddCard(Cards card)
        {
            hand.Add(card);
        }

        public Cards RemoveCard()
        {
            if (hand.Count == 0)
                return Cards.None;
            Cards card = hand[0];
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

        public void SetCardDrawn(Cards card)
        {
            CardDrawn = card;
        }

        public Cards GetCardDrawn()
        {
            return CardDrawn;
        }

        public void Write(string msg)
        {
            Connect.SendObject("Message", msg);
        }

        public void SendCmd(Cmd command, List<Cards> list = null)
        {
            ProtocolCl cmd = new ProtocolCl(command, list);
            Connect.SendObject("Protocol", cmd);
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
