using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    public class ItemCommandsHandler : MonoBehaviour
    {
        SeedsService _seedsService;
        TerritoryService _territoryService;
        [Inject]
        public void Constructor(SeedsService seedsService, TerritoryService territoryService)
        {
            _seedsService = seedsService;
            _territoryService = territoryService;
        }
        private void Start()
        {
            CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        }
        private void OnDisable()
        {
            CommunicationEvents.OnStartCommunicateEvent -= OnStartCommunicate;
        }
        private void Update()
        {
            if (User.IsConnectionCreated)
            {
                HandleCommands();
                DeleteComplitedCommands();
            }
        }
        
        void HandleCommands()
        {
            if (Communicator.RecvData.ItemCommands.Count > 0)
            {
                List<ItemCommand> commands = Communicator.RecvData.ItemCommands;
                for (int i = 0; i < commands.Count; i++)
                {
                    switch(commands[i].CommandType)
                    {
                        case CommandType.Spawn:
                            {
                                Seed seedPrefab = _seedsService.GetSeedFor(commands[i].ObjectType);
                                SeedSO seedSO = _seedsService.GetSeedSOFor(commands[i].ObjectType);

                                PlantTerritory terr =
                                    _territoryService.GetTerritiryByObjectName(commands[i].ParentTerritoryName);

                                if (terr.IsEmpty == true)
                                {
                                    seedPrefab = Instantiate(seedPrefab,
                                        terr.transform);

                                    seedPrefab.Initialize(seedSO, terr, commands[i].PlayerType);
                                    terr.SetEmpty(false);
                                    terr.SetSeed(seedPrefab);
                                    Communicator.SendData.AddComplitedCommand(commands[i]);
                                    
                                }
                                break;
                            }
                        case CommandType.Delete:
                            {
                                PlantTerritory terr =
                                    _territoryService.GetTerritiryByObjectName(commands[i].ParentTerritoryName);
                                terr.DestroySeed();
                                terr.SetEmpty(true);
                                Communicator.SendData.AddComplitedCommand(commands[i]);
                                break;
                            }
                    }
                    commands.RemoveAt(i);

                }
            }
        }

        public void DeleteComplitedCommands()
        {
            if (Communicator.SendData.ItemCommands.Count > 0)
            {

                List<ItemCommand> sendCommands = Communicator.SendData.ItemCommands;

                List<ItemCommand> recvComplitedCommands = Communicator.RecvData.CompletedCommands;

                DeleteIntersect(sendCommands, recvComplitedCommands);
                DeleteIntersect(recvComplitedCommands, sendCommands);
            }
        }

        void DeleteIntersect<T>(List<T> first, List<T> second)
        {
            List<T> common = first.Intersect(second).ToList();
            first.RemoveAll(item => common.Contains(item));
            second.RemoveAll(item => common.Contains(item));
        }

        async void ClearComplitedCommands()
        {
            while(User.IsConnectionCreated)
            {
                await Task.Delay(10000);
                Communicator.SendData.CompletedCommands.Clear();
            }
        }

        void OnStartCommunicate()
        {
            Task.Factory.StartNew(ClearComplitedCommands);
        }

    }
}
