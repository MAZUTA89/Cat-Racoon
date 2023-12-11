﻿using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Start()
        {
            CommunicationEvents.oncomplitedAdded += OnComplitedAdded;
        }
        private void OnDisable()
        {
            CommunicationEvents.oncomplitedAdded -= OnComplitedAdded;
        }
        private void Update()
        {
            if (User.IsConnectionCreated)
            {
                //Debug.Log(Communicator.RecvData.ItemCommands.Count);
                HandleCommands();
                DeleteComplitedCommands();
            }
        }
        void OnComplitedAdded()
        {
            Debug.Log("Add Comlited!");
        }
        //передовать объект во имени. Если эта платформа уже занята, то ничего не ставить
        void HandleCommands()
        {
            if (Communicator.RecvData.ItemCommands.Count > 0)
            {
                List<ItemCommand> commands = Communicator.RecvData.ItemCommands;
                List<string> NotFreeTerr = Communicator.RecvData.NotFreeTerritoryList;
                for (int i = 0; i < commands.Count; i++)
                {
                    Seed seedPrefab = _seedsService.GetSeedFor(commands[i].ObjectType);
                    SeedSO seedSO = _seedsService.GetSeedSOFor(commands[i].ObjectType);

                    PlantTerritory terr = 
                        _territoryService.GetTerritiryByObjectName(commands[i].ParentTerritoryName);

                    if (terr.IsEmpty == true)
                    {
                        seedPrefab = Instantiate(seedPrefab,
                            terr.transform);

                        seedPrefab.Initialize(seedSO);
                        terr.SetEmpty(false);
                        Communicator.SendData.AddComplitedCommand(commands[i]);
                        Communicator.SendData.AddNotFreeTerritory(commands[i].ParentTerritoryName);
                        Debug.Log("Добавил в Send готовую");
                        commands.RemoveAt(i);
                    }
                }
            }
        }

        public void DeleteComplitedCommands()
        {
            if (Communicator.SendData.ItemCommands.Count > 0)
            {

                List<ItemCommand> sendCommands = Communicator.SendData.ItemCommands;

                List<ItemCommand> recvComplitedCommands = Communicator.RecvData.CompletedCommands;

                List<ItemCommand> commonCommands = sendCommands.Intersect(recvComplitedCommands).ToList();

                sendCommands.RemoveAll(item => commonCommands.Contains(item));
                recvComplitedCommands.RemoveAll(item => commonCommands.Contains(item));
            }
        }




    }
}
