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

        public PlayerData()
        {
            random = new Random();
        }
        public void UpdatePosition()
        {
           PositionX = random.NextDouble();
        }

        public Vector2 GetPosition()
        {
            Vector2 position = new Vector2(PositionX, PositionY);
            return position;
        }
         
    }
}
