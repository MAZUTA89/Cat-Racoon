using System.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace Assets.Code.Scripts.Lobby
{
    public class LobbyService : MonoBehaviour
    {
        GameObject _startPanel;
        GameObject _createPanel;
        GameObject _connectPanel;
        GameObject _createConnectionStringPanel;
        LobbyConnection _connection;
        GameObject _connectCancelButton;
        [Inject]
        public void Constructor(
            [Inject(Id = "StartPanel")] GameObject startPanel,
            [Inject(Id = "CreatePanel")] GameObject createPanel,
            [Inject(Id = "ConnectPanel")] GameObject connectPanel,
            [Inject(Id = "ConnectionStringPanel")] GameObject connectionStringPanel,
            [Inject(Id = "ConnectCancelButton")] GameObject connectCancelButton,
            [Inject(Id = "CreateCancelButton")] GameObject createCancelButton
            )
        {
            _startPanel = startPanel;
            _createPanel = createPanel;
            _connectPanel = connectPanel;
            _createConnectionStringPanel = connectionStringPanel;
            _connectCancelButton = connectCancelButton;
        }

        private void Start()
        {
            _connection = GetComponent<LobbyConnection>();
            _connection.OnCancelClientConnectionEvent += ConnectBack;
            _connection.OnCancelServerConnectionEvent += CreateBack;
        }
        private void OnDisable()
        {
            _connection.OnCancelClientConnectionEvent -= ConnectBack;
            _connection.OnCancelServerConnectionEvent -= CreateBack;
        }
        public void OnConnect()
        {
            _startPanel.SetActive(false);
            _connectPanel.SetActive(true);
            _createConnectionStringPanel.SetActive(true);
        }
        public void OnCreate()
        {
            _startPanel.SetActive(false);
            _createPanel.SetActive(true);
        }
        public void OnExit()
        {
            Application.Quit();
        }
        public async void ConnectBack()
        {
            await Task.Delay(1000);
            _connectPanel?.SetActive(false);
            _startPanel?.SetActive(true);
        }
        public async void CreateBack()
        {
            _createPanel?.SetActive(false);
            await Task.Delay(1000);
            _startPanel?.SetActive(true);
        }
        public void OnBackAtCreateConnectionStringPanel()
        {
            _createConnectionStringPanel.SetActive(false);
            _connectPanel.SetActive(false);
            _startPanel.SetActive(true);
        }
        public void OnContinue()
        {
            _createConnectionStringPanel.SetActive(false);
            _connectCancelButton.SetActive(true);
        }
    }
}
