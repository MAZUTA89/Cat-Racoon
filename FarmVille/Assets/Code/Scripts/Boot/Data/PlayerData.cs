using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public PlayerData() 
        {
        }
        public bool IsSpawning;
        public float positionX;
        public float positionY;
        public void UpdatePosition(Vector2 position)
        {
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
