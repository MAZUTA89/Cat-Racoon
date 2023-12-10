using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public enum Item : int
    {
        Wheat = 1,
        Pumking = 2
    }

    public class SpawnCommand
    {
        public float PositionX;
        public float PositionY;
        public Item ItemType;
        public Vector2 GetPosition()
        {
            return new Vector2(PositionX, PositionY);
        }

        public Item GetItemType()
        {
            return ItemType;
        }
    }
}
