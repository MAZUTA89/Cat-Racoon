using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.Code.Scripts.Gameplay
{
    public class Inventory
    {
        Dictionary<Item, int> _itemsDictionary;

        public Inventory()
        {
            _itemsDictionary = new Dictionary<Item, int>();
            InitItemsDictionaryRandom();
        }

        public int this[Item item]
        {
            get
            {
                {
                    return _itemsDictionary[item];
                }
            }
            set
            {
                _itemsDictionary[item] = value;
            }
        }

        void InitItemsDictionaryRandom()
        {
            Item[] items = (Item[])Enum.GetValues(typeof(Item));

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != Item.Basket)
                {
                    _itemsDictionary[items[i]] = Random.Range(2, 5);
                }
            }
        }
    }
}
