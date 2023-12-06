using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Gameplay.Player.PlayerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class PlayersInstaller : MonoInstaller
    {
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform HostSpawnPoint;
        [SerializeField] GameObject ConnectedPlayerPrefab;
        [SerializeField] Transform ConnectedPlayerSpawnPoint;
        public override void InstallBindings()
        {
            BindSpawnPoints();
            BindMovement();

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

        void BindSpawnPoints()
        {
            Container.BindInstance(ConnectedPlayerSpawnPoint).WithId("ConnectedSpawn");
            Container.BindInstance(HostSpawnPoint).WithId("HostSpawn");
        }
        void BindMovement()
        {
            Container.Bind<PlayerMovement>().AsSingle();
        }
    }
}
