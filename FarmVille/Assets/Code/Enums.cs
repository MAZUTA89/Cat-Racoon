using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code
{
    public enum ConnectionType
    {
        Server,
        Client
    }

    public enum SceneName
    {
        Farm
    }

    public enum CommandType
    {
        Spawn,
        Delete
    }

    public enum Item
    {
        Basket,
        Watering,
        Wheat,
        Pumpking,
        SunFlower,
        InfernalGrowth,
        Reed
    }
    public enum GrowStatus
    {
        Growing,
        Ready
    }

    public enum Tool
    {
        Basket,
        Watering
    }

   public enum Crops
    {
        Wheat,
        Pumpking,
        SunFlower,
        InfernalGrowth,
        Reed
    }

    public enum PlayerMode
    {
        Single,
        Multiple
    }

}
