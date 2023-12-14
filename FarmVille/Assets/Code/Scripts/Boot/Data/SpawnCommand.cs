using System;

namespace Assets.Code.Scripts.Boot.Data
{
    public class ItemCommand
    {
        public Item ObjectType;
        public CommandType CommandType;
        public PlayerType PlayerType;
        
        public string ParentTerritoryName;
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ItemCommand command = (ItemCommand)obj;

            return command.ObjectType == ObjectType
                && command.ParentTerritoryName == ParentTerritoryName
                && command.CommandType == CommandType
                && command.PlayerType == PlayerType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectType, ParentTerritoryName, CommandType, PlayerType);
        }
    }
}
