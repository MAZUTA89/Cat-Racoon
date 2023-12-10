using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class ItemCommand
    {
        public Item ObjectType;
        public float PositionX;
        public float PositionY;
        public void SetPosition(Vector2 position)
        {
            PositionX = position.x;
            PositionY = position.y;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(PositionX, PositionY);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ItemCommand command = (ItemCommand)obj;

            return command.ObjectType == ObjectType
                && command.PositionX == PositionX 
                && command.PositionY == PositionY;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PositionX, PositionY, ObjectType);
        }
    }
}
