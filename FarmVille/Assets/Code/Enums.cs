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

    public enum ObjectType
    {
        Cube
    }
    public enum CellType
    {
        Basket,
        Watering,
        Wheat,
        Pumpking,
        SunFlower,
        InfernalGrowth,
        Reed
    }
    public enum Seed
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
