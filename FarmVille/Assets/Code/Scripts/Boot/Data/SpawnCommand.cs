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
        public bool IsDone;
        public void SetPosition(Vector2 position)
        {
            PositionX = position.x;
            PositionY = position.y;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(PositionX, PositionY);
        }
    }
}
