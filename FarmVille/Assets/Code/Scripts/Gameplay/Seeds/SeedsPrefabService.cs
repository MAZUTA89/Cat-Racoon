using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay
{
    public class SeedsService
    {
        Dictionary<Item, Seed> _seedsPrefabs;

        Dictionary<Item, SeedSO> _seedsSoDictionary;

        public SeedsService(Dictionary<Item, Seed> seedsPrefabs, Dictionary<Item, SeedSO> seedsSoDictionary)
        {
            _seedsPrefabs = seedsPrefabs;
            _seedsSoDictionary = seedsSoDictionary;
        }

        public Seed GetSeedFor(Item seed)
        {
            return _seedsPrefabs[seed];
        }
        public SeedSO GetSeedSOFor(Item seed)
        {
            return _seedsSoDictionary[seed];
        }


    }
}
