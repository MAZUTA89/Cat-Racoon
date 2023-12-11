using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;
using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;

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
        List<ItemCommand> _sendedCommands;
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
            _cursorRay.OnHitTerritoryEvent -= OnHitTerritory2DCollider;
            _cursorRay.OnHitSeedEvent -= OnHitSeed2DCollider;
            _inventoryInputService.OnChooseCellTypeEvent -= OnChooseCell;
        }
        private void Start()
        {
            _sendedCommands = new List<ItemCommand>();
            _cursorRay = GetComponent<CursorRay>();
            _cursorRay.OnHitTerritoryEvent += OnHitTerritory2DCollider;
            _cursorRay.OnHitSeedEvent += OnHitSeed2DCollider;
            _inventoryInputService.OnChooseCellTypeEvent += OnChooseCell;
        }

        private void Update()
        {
        }
        public void OnHitTerritory2DCollider(RaycastHit2D hit)
        {
            if (hit.distance < Distance)
            {
                GameObject go = hit.collider.gameObject;
                UsingCrops(go);
            }
        }
        public void OnHitSeed2DCollider(RaycastHit2D hit)
        {
            if (hit.distance < Distance)
            {
                GameObject go = hit.collider.gameObject;
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

                seedPrefab.Initialize(seedSO, territory, User.Instance.PlayerType);
                territory.SetSeed(seedPrefab);

                territory.SetEmpty(false);

                _inventory[_cuurentChoosenItem]--;

                if (User.IsConnectionCreated)
                {
                    Communicator.SendData.AddNotFreeTerritory(territory.name);
                    ItemCommand itemCommand = new ItemCommand();
                    itemCommand.ObjectType = _cuurentChoosenItem;
                    itemCommand.ParentTerritoryName = territory.name;
                    itemCommand.CommandType = CommandType.Spawn;
                    itemCommand.PlayerType = User.Instance.PlayerType;
                    _sendedCommands.Add(itemCommand);
                    Communicator.SendData.AddItemCommand(itemCommand);
                }
            }
        }

        void UsingTools(GameObject go)
        {
            if (go.TryGetComponent(out Seed seed))
            {
                switch (_cuurentChoosenItem)
                {
                    case Item.Basket:
                        {
                            if (seed.Status == GrowStatus.Ready
                                && seed.ParentPlayer == User.Instance.PlayerType)
                            {
                                GameEvents.InvokePickSeedEvent(seed.Money);
                                _inventory[Item.Watering] += Random.Range(0, 2);
                                _inventory[seed.GetSeedType()] += Random.Range(1, 3);
                                seed.OnPick();
                                Destroy(seed.gameObject);

                                if (User.IsConnectionCreated)
                                {
                                    ItemCommand itemCommand = new ItemCommand();
                                    itemCommand.ParentTerritoryName = seed.GetParentTerritoryName();
                                    itemCommand.CommandType = CommandType.Delete;
                                    itemCommand.ObjectType = _cuurentChoosenItem;
                                    Communicator.SendData.AddItemCommand(itemCommand);
                                    Communicator.SendData.NotFreeTerritoryList
                                        .Remove(seed.GetParentTerritoryName());
                                }
                            }

                            break;
                        }
                    case Item.Watering:
                        {
                            if (_inventory[_cuurentChoosenItem] < 1)
                                break;

                            if (seed.Status == GrowStatus.Growing)
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
