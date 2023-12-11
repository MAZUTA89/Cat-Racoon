using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class PlayerData
    {
        public List<ItemCommand> ItemCommands;
        public List<ItemCommand> CompletedCommands;
        public List<String> NotFreeTerritoryList;
        public float DirectionX;
        public float DirectionY;
        public PlayerData() 
        {
            ItemCommands = new List<ItemCommand>();
            CompletedCommands = new List<ItemCommand>();
            NotFreeTerritoryList = new List<String>();
        }
        public bool IsLeftButton;
        public float Money;
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
        public void AddItemCommand(ItemCommand itemCommand)
        {
            ItemCommands.Add(itemCommand);
        }
        public void AddComplitedCommand(ItemCommand itemCommand)
        {
           CompletedCommands.Add(itemCommand);
        }
        public void AddNotFreeTerritory(string name)
        {
            NotFreeTerritoryList.Add(name);
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
