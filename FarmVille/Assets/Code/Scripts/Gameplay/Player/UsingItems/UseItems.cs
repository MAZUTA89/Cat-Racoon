﻿using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    [RequireComponent(typeof(CursorRay))]
    public class UseItems : MonoBehaviour
    {
        [SerializeField] float Distance = 30f;
        
        CursorRay _cursorRay;
        Item _cuurentChoosenItem;
        InventoryInputService _inventoryInputService;
        SeedsService _seedsService;
        Inventory _inventory;
        [Inject]
        public void Constructor(InventoryInputService inventoryInputService,
            SeedsService seedsService, Inventory inventory)
        {
            _inventoryInputService = inventoryInputService;
            _seedsService = seedsService;
            _inventory = inventory;
        }
       
        private void OnDisable()
        {
            _cursorRay.OnHitEvent -= OnHit2DCollider;
            _inventoryInputService.OnChooseCellTypeEvent -= OnChooseCell;
        }
        private void Start()
        {
            _cursorRay = GetComponent<CursorRay>();
            _cursorRay.OnHitEvent += OnHit2DCollider;
            _inventoryInputService.OnChooseCellTypeEvent += OnChooseCell;
        }

        private void Update()
        {
        }
        public void OnHit2DCollider(RaycastHit2D hit)
        {
            if (hit.distance < Distance)
            {
                GameObject go = hit.collider.gameObject;
                UsingCrops(go);
                UsingTools(go);
            }
        }

        void UsingCrops(GameObject go)
        {
            if (go.TryGetComponent(out PlantTerritory territory))
            {
                if (territory.IsTerritoryContain(_cuurentChoosenItem) == false ||
                    territory.IsEmpty == false ||
                    _inventory[_cuurentChoosenItem] < 1)
                {
                    return;
                }

                Seed seedPrefab = _seedsService.GetSeedFor(_cuurentChoosenItem);
                SeedSO seedSO = _seedsService.GetSeedSOFor(_cuurentChoosenItem);

                seedPrefab = Instantiate(seedPrefab, territory.transform);

                seedPrefab.Initialize(seedSO, territory);

                territory.SetEmpty(false);

                _inventory[_cuurentChoosenItem]--;
            }
        }

        void UsingTools(GameObject go)
        {
            if(go.TryGetComponent(out Seed seed))
            {
                switch (_cuurentChoosenItem)
                {
                    case Item.Basket:
                        {
                            if(seed.Status == GrowStatus.Ready)
                            {
                                GameEvents.InvokePickSeedEvent(seed.Money);
                                _inventory[Item.Watering] += Random.Range(0, 2);
                                _inventory[seed.GetSeedType()] += Random.Range(1, 3);
                                seed.OnPick();
                                Destroy(seed.gameObject);
                            }
                            break;
                        }
                    case Item.Watering:
                        {
                            if (_inventory[_cuurentChoosenItem] < 1)
                                break;

                            if(seed.Status == GrowStatus.Growing)
                            {
                                seed.Boost();
                                _inventory[_cuurentChoosenItem]--;
                            }

                            break;
                        }
                }
            }
        }
        void OnChooseCell(Item item)
        {
            _cuurentChoosenItem = item;
        }
    }
}
