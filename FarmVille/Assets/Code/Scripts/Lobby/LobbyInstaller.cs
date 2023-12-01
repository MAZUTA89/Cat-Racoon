using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Lobby
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] GameObject StartPanel;
        [SerializeField] GameObject CreatePanel;
        [SerializeField] GameObject ConnectPanel;
        public TextMeshProUGUI ProcessText;
        [SerializeField] GameObject LoadImage;
        public override void InstallBindings()
        {
            Container.Bind<LobbyService>().AsSingle();
            Container.Bind<LobbyConnection>().AsSingle();
            Container.Bind<LevelLoader>().AsSingle();
            Container.Bind<ProgressHandler>().AsSingle();
            BindStartPanel();
            BindConnectPanel();
            BindCreatePanel();
            BindProgressUI();
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
        void BindProgressUI()
        {
            Container.BindInstance(ProcessText).WithId("ConnectionProgressText");
            Container.BindInstance(LoadImage).WithId("ConnectionLoadImage");
        }
    }
}
