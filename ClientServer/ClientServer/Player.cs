using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public struct Position
    {
        public int x;
        public int y;
    }
    public class Player
    {
        public Position pos;
        public List<Person> people;
        public Player()
        {
            people = new List<Person>();
        }
        public void Add(Person p)
        {
            people.Add(p);
        }
    }
}
