using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cardGame_Server
{
    class Client
    {
        private List<Card> hand = new List<Card>();
        private string Name { get; set; }
        private int Id { get; set; }
   
        Client(string name)
        {
            Name = name;
        }
    }
}
