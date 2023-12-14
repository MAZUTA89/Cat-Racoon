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
        [SerializeField] GameObject ConnectionStringPanel;
        [SerializeField] GameObject ConnectCancelButton;
        [SerializeField] GameObject CreateCancelButton;
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
            BindConnectionStringPanel();
            BindProgressUI();
            BindCancelButtons();
        }
        void BindCancelButtons()
        {
            Container.BindInstance(ConnectCancelButton).WithId("ConnectCancelButton");
            Container.BindInstance(CreateCancelButton).WithId("CreateCancelButton");
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
        void BindConnectionStringPanel()
        {
            Container.BindInstance(ConnectionStringPanel).WithId("ConnectionStringPanel");
        }
        void BindProgressUI()
        {
            Container.BindInstance(ProcessText).WithId("ConnectionProgressText");
            Container.BindInstance(LoadImage).WithId("ConnectionLoadImage");
        }
    }
}
