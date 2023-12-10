using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace ClientServer
{
    public class PlayerData
    {
        public double PositionX;
        public double PositionY;
        Random random;
        public List<SpawnCommand> spawnCommands;

        public PlayerData()
        {
            random = new Random();
            spawnCommands = new List<SpawnCommand>();
            SpawnCommand spawnCommand = new SpawnCommand();
            SpawnCommand spawnCommand1 = new SpawnCommand();
            spawnCommands.Add(spawnCommand);
            spawnCommands.Add(spawnCommand1);
        }
        public void UpdateItem()
        {
            foreach (var spawnCommand in spawnCommands)
            {
                spawnCommand.ItemType = (Item)random.Next(1, 3);
            }
        }
        public void UpdatePosition()
        {
           PositionX = random.NextDouble();
        }
        
        public List<Item> GetItemType()
        {
            List<Item> items = new List<Item>();
            foreach (var spawnCommand in spawnCommands)
            {
                items.Add(spawnCommand.ItemType);
            }
            return items;
        }

        public Vector2 GetPosition()
        {
            Vector2 position = new Vector2(PositionX, PositionY);
            return position;
        }
         
    }
}
