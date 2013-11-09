using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cross_Zero.Network;

namespace Cross_Zero.Logic
{
    public class Player
    {
        //Network
        public StateObject NetState { get; set; }

        //Logic
        public int Id { get; set; }
        public string Sign { get; set; }
        public string Name { get; set; }
        public int ActivatedRects { get; set; }

        public Player(int id, string name, string sign)
        {
            Id = id;
            Name = name;
            Sign = sign;
        }
    }
}
