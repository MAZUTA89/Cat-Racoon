using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Data
{
    public class ItemCommand
    {
        public Item ObjectType;
        
        public string ParentTerritoryName;
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ItemCommand command = (ItemCommand)obj;

            return command.ObjectType == ObjectType
                && command.ParentTerritoryName == ParentTerritoryName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectType, ParentTerritoryName);
        }
    }
}
