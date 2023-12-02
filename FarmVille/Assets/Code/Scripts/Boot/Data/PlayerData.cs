using System;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public bool HasChanges { get; private set; }

        Vector2 _position;
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
