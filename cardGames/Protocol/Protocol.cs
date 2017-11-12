using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Protocol
{
    public enum Cmd
    {
        Ready = 0xff00,
        Games_list = 0xff01,
        BJ = 0xff02,
        Bataille = 0xff03,
        Hand = 0xff04,
        Turn = 0xff05,
        Win = 0xff06,
        Lose = 0xff07,
        Draw = 0xff08,
        //DrawCard,
    }

    public enum Cards { None=-1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

    [ProtoContract]
    public class ProtocolCl
    {
        [ProtoMember(1)]
        public Cmd Command { get; private set; }

        [ProtoMember(2)]
        public List<Cards> CardSend { get; private set; }

        protected ProtocolCl() { }

        public ProtocolCl(Cmd cmd, List<Cards> cards = null)
        {
            Command = cmd;
            CardSend = cards;
        }
    }
}
