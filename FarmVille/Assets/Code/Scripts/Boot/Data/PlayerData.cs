using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public PlayerData() 
        {
            HasChanges = true;
        }
        public bool HasChanges { get; private set; }

        public float positionX;
        public float positionY;
        public void UpdatePosition(Vector2 position)
        {
            Vector2 curr = new Vector2(positionX, positionY);
            if (curr != position)
            {
                HasChanges = true;
            }
            else HasChanges = false;

            positionX = position.x;
            positionY = position.y;
        }
        public Vector2 GetPosition()
        {
            Vector2 pos = new Vector2(positionX, positionY);
            return pos;
        }

    }
}
