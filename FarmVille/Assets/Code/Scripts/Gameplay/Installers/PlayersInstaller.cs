using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Gameplay.Player.PlayerControl;
using UnityEngine;
using Zenject;
using Cinemachine;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class PlayersInstaller : MonoInstaller
    {
        [SerializeField] GameObject PlayerPrefab;
        [SerializeField] Transform HostSpawnPoint;
        [SerializeField] GameObject ConnectedPlayerPrefab;
        [SerializeField] Transform ConnectedPlayerSpawnPoint;

        public PlayerMode Mode;
        [SerializeField] CinemachineVirtualCamera Camera;
        GameObject _playerGameoObject;
        public override void InstallBindings()
        {
            BindSpawnPoints();
            BindMovement();

            switch (Mode)
            {
                case PlayerMode.Single:
                    {
                        SingleMode();
                        break;
                    }
                case PlayerMode.Multiple:
                    {

                        SpawnPlayersOnSpawnPoints();
                        break;
                    }
            }
        }
        void SpawnPlayersOnSpawnPoints()
        {
            switch (User.Instance.ConnectionType)
            {
                case ConnectionType.Server:
                    {
                        _playerGameoObject = Container.InstantiatePrefab(PlayerPrefab,
                            HostSpawnPoint.position,
                            Quaternion.identity, null);
                        Container.InstantiatePrefab(ConnectedPlayerPrefab,
                            ConnectedPlayerSpawnPoint.position,
                            Quaternion.identity, null);
                        break;
                    }
                case ConnectionType.Client:
                    {
                        _playerGameoObject = Container.InstantiatePrefab(PlayerPrefab,
                            ConnectedPlayerSpawnPoint.position,
                            Quaternion.identity, null);
                        Container.InstantiatePrefab(ConnectedPlayerPrefab,
                           HostSpawnPoint.position,
                           Quaternion.identity, null);
                        break;
                    }
            }
            if (_playerGameoObject != null)
            {
                Camera.Follow = _playerGameoObject.transform;
                Camera.LookAt = _playerGameoObject.transform;
            }

        }
        void SingleMode()
        {
            _playerGameoObject = Container.InstantiatePrefab(PlayerPrefab, HostSpawnPoint.position, Quaternion.identity, null);
            Camera.Follow = _playerGameoObject.transform;
            Camera.LookAt = _playerGameoObject.transform;
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
