using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Assets.Code.Scripts.Boot;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class SpawnInstaller : MonoInstaller
    {
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform HostSpawnPoint;
        [SerializeField] GameObject ConnectedPlayerPrefab;
        [SerializeField] Transform ConnectedPlayerSpawnPoint;

        public override void InstallBindings()
        {
            
            switch (User.Instance.ConnectionType)
            {
                case ConnectionType.Server:
                    {
                        Container.InstantiatePrefab(PlayerPrefab,
                            HostSpawnPoint.position,
                            Quaternion.identity, null);
                        Container.InstantiatePrefab(ConnectedPlayerPrefab,
                            ConnectedPlayerSpawnPoint.position,
                            Quaternion.identity, null);
                        break;
                    }
                case ConnectionType.Client:
                    {
                        Container.InstantiatePrefab(PlayerPrefab,
                            ConnectedPlayerSpawnPoint.position,
                            Quaternion.identity, null);
                        Container.InstantiatePrefab(ConnectedPlayerPrefab,
                           HostSpawnPoint.position,
                           Quaternion.identity, null);
                        break;
                    }
            }
        }
    }
}
