﻿using System;
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
        public List<String> NotEmptyTerritoryList;
        public PlayerData() 
        {
            ItemCommands = new List<ItemCommand>();
            CompletedCommands = new List<ItemCommand>();
            NotEmptyTerritoryList = new List<String>();
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
        public void AddNotEmptyTerritory(string name)
        {
            NotEmptyTerritoryList.Add(name);
        }
    }
}
