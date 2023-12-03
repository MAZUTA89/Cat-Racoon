using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public PlayerData() 
        {
            _position = new Vector2(1, 1);
        }
        public bool HasChanges { get; private set; }

        private Vector2 _position;
        public void UpdatePosition(Vector2 position)
        {
            if (_position != position)
            {
                HasChanges = true;
            }
            else HasChanges = false;

            _position = position;
        }
        public Vector2 GetPosition()
        {
            return _position;
        }

    }
}
