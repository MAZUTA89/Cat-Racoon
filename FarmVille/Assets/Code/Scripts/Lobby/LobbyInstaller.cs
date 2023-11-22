using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Lobby
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] GameObject StartPanel;
        [SerializeField] GameObject CreatePanel;
        [SerializeField] GameObject ConnectPanel;
        public override void InstallBindings()
        {
            Container.Bind<LobbyService>().AsSingle();
            BindStartPanel();
            BindConnectPanel();
            BindCreatePanel();
        }

        void BindStartPanel()
        {
            Container.BindInstance(StartPanel).WithId("StartPanel");
        }
        void BindConnectPanel()
        {
            Container.BindInstance(CreatePanel).WithId("CreatePanel");
        }
        void BindCreatePanel()
        {
            Container.BindInstance(ConnectPanel).WithId("ConnectPanel");
        }
    }
}
