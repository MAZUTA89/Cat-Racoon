using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using System;
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
        [Inject]
        public void Constructor(SeedsService seedsService)
        {
            _seedsService = seedsService;
        }
        private void Update()
        {
            if(User.IsConnectionCreated)
            {
                Debug.Log(Communicator.RecvData.ItemCommands.Count);
                HandleCommands();
            }
        }

        void HandleCommands()
        {
            if(Communicator.RecvData.ItemCommands.Count > 0)
            {
                Debug.Log("Connected Left!");
                List<ItemCommand> commands = Communicator.RecvData.ItemCommands;

                for (int i = 0; i < commands.Count; i++)
                {
                    Seed seedPrefab = _seedsService.GetSeedFor(commands[i].ObjectType);
                    SeedSO seedSO = _seedsService.GetSeedSOFor(commands[i].ObjectType);

                    seedPrefab = Instantiate(seedPrefab, commands[i].GetPosition(), Quaternion.identity);

                    seedPrefab.Initialize(seedSO);
                    commands[i].IsDone = true;

                    commands.RemoveAt(i);
                }

                //ClearIfDone(commands);
                //PerformCommands(commands);
            }
        }

        void ClearIfDone(List<ItemCommand> commands)
        {
            for(int i = 0; i < commands.Count; i++)
            {
                if (commands[i].IsDone)
                {
                    commands.RemoveAt(i);
                }
            }
        }

        void PerformCommands(List<ItemCommand> commands)
        {
            foreach(ItemCommand command in commands)
            {
                Seed seedPrefab = _seedsService.GetSeedFor(command.ObjectType);
                SeedSO seedSO = _seedsService.GetSeedSOFor(command.ObjectType);

                seedPrefab = Instantiate(seedPrefab, command.GetPosition(), Quaternion.identity);

                seedPrefab.Initialize(seedSO);
                command.IsDone = true;
            }
        }
    }
}
