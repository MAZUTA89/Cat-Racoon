﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public List<ItemCommand> ItemCommands;
        public List<ItemCommand> CompletedCommands;
        public float DirectionX;
        public float DirectionY;
        public bool IsLeftButton;
        public float Money;
        public float positionX;
        public float positionY;
        public float MovementSpeed;
        public PlayerData() 
        {
            ItemCommands = new List<ItemCommand>();
            CompletedCommands = new List<ItemCommand>();
        }
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
        public void AddItemCommand(ItemCommand itemCommand)
        {
            ItemCommands.Add(itemCommand);
        }
        public void AddComplitedCommand(ItemCommand itemCommand)
        {
           CompletedCommands.Add(itemCommand);
        }
       
        public void SetDirection(Vector2 direction)
        {
            DirectionX = direction.x;
            DirectionY = direction.y;
        }
        public Vector2 GetDirection()
        {
            return new Vector2(DirectionX, DirectionY);
        }
    }
}
