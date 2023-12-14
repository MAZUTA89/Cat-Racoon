using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Lobby
{
    public class ProgressHandler : MonoBehaviour
    {
        const string c_ConnectionText = "Waiting for connection...";
        const string c_SuccsessConnectionText = "Connection is established";
        const string c_ConnectionFailedText = "Connection failed";
        const string c_CanceledText = "Canceled...";
        const string c_LoadLevelText = "Loading level ...";
        const string c_CheckText = "Check signal ...";
        const string c_CheckSignalFailedText = "Check signal failed!";
        const string c_FailedCreateConnectionStringText = "Error!";

        LobbyConnection _lobbyConnection;
        LevelLoader _levelLoader;
        TextMeshProUGUI _processText;
        [SerializeField] TextMeshProUGUI _endPointText;
        GameObject _loadImage;
        GameObject _connectCancelButton;
        GameObject _createCancelButton;
        GameObject _connectionStringPanel;
        [Inject]
        public void Constructor(
            [Inject(Id = "ConnectionProgressText")] TextMeshProUGUI progressText,
            [Inject(Id = "ConnectionLoadImage")] GameObject loadImage,
            [Inject(Id = "ConnectCancelButton")] GameObject connectCancelButton,
            [Inject(Id = "CreateCancelButton")] GameObject createCancelButton,
            [Inject(Id = "ConnectionStringPanel")] GameObject connectionStringPanel)
        {
            _processText = progressText;
            _loadImage = loadImage;
            _connectCancelButton = connectCancelButton;
            _createCancelButton = createCancelButton;
            _connectionStringPanel = connectionStringPanel;
        }

        private void Start()
        {
            _levelLoader = GetComponent<LevelLoader>();
            _lobbyConnection = GetComponent<LobbyConnection>();
            _lobbyConnection.OnStartCreateConnectionEvent 
                += OnStartCreateConnection;
            _lobbyConnection.OnCancelClientConnectionEvent 
                += OnCancelClientConnection;
            _lobbyConnection.OnCancelServerConnectionEvent 
                += OnCancelServerConnection;
            _lobbyConnection.OnCreateServerEndPointEvent +=
                OnCreateServerEndPoint;
            _lobbyConnection.OnCreateConnectionStringFailedEvent +=
                OnCreateConnectionStringFailedEvent;
            _levelLoader.OnCanceledOrFailedLoadLevelSignalEvent 
                += OnCanceledOrFailedLoadLevelSignal;
            _levelLoader.OnStartCheckLoadLevelSignalEvent 
                += OnStartCheckLoadLevelSignal;
            _levelLoader.OnStopCheckLoadLevelSignalEvent 
                += OnStopCheckLoadLevelSignal;
            _lobbyConnection.OnCreateConnectionSeccessEvent
                += OnCreateConnectionSeccess;
            _lobbyConnection.OnCreateConnectionFailedEvent
                += OnCreateConnectionFailed;
        }

        private void OnDisable()
        {
            _lobbyConnection.OnStartCreateConnectionEvent
                -= OnStartCreateConnection;
            _lobbyConnection.OnCancelClientConnectionEvent 
                -= OnCancelClientConnection;
            _lobbyConnection.OnCancelServerConnectionEvent
                -= OnCancelServerConnection;
            _lobbyConnection.OnCreateServerEndPointEvent -=
                OnCreateServerEndPoint;
            _lobbyConnection.OnCreateConnectionSeccessEvent
                -= OnCreateConnectionSeccess;
            _lobbyConnection.OnCreateConnectionFailedEvent
                -= OnCreateConnectionFailed;
            _lobbyConnection.OnCreateConnectionStringFailedEvent -=
                OnCreateConnectionStringFailedEvent;
            _levelLoader.OnCanceledOrFailedLoadLevelSignalEvent
                -= OnCanceledOrFailedLoadLevelSignal;
            _levelLoader.OnStartCheckLoadLevelSignalEvent
                -= OnStartCheckLoadLevelSignal;
            _levelLoader.OnStopCheckLoadLevelSignalEvent
                -= OnStopCheckLoadLevelSignal;
        }
        public void OnCanceledOrFailedLoadLevelSignal()
        {
            _connectCancelButton.SetActive(true);
            _createCancelButton.SetActive(true);
            _processText.gameObject.SetActive(true);
            _processText.text = c_CheckSignalFailedText;
            Task.Delay(1000);
            _processText.gameObject.SetActive(false);
        }
        public void OnStartCheckLoadLevelSignal()
        {
            _endPointText.gameObject.SetActive(false);
            _processText.text = c_CheckText;
        }
        public void OnStopCheckLoadLevelSignal()
        {
            _connectCancelButton.SetActive(false);
            _createCancelButton.SetActive(false);
            _processText.text = c_LoadLevelText;
            _processText.gameObject.SetActive(false);
            _loadImage.SetActive(false);
        }
        public async void OnCancelServerConnection()
        {
            _processText.text = c_CanceledText;
            await Task.Delay(1000);
            DeactivateProgressUI();
        }
        public async void OnCancelClientConnection()
        {
            _processText.text = c_CanceledText;
            await Task.Delay(1000);
            DeactivateProgressUI();
        }
        public void OnStartCreateConnection()
        {
            ActivateProgressUI();
            _processText.text = c_ConnectionText;
        }
        public void OnCreateConnectionSeccess()
        {
            _processText.text = c_SuccsessConnectionText;
        }
        public void OnCreateConnectionFailed()
        {
            _processText.text = c_ConnectionFailedText;
            _processText.gameObject.SetActive(false);
            
        }
        public void OnCreateServerEndPoint(string endPoint)
        {
            _endPointText.gameObject.SetActive(true);
            _endPointText.text = endPoint;
        }
        public async void OnCreateConnectionStringFailedEvent()
        {
            _processText.gameObject.SetActive(true);
            _processText.text = c_FailedCreateConnectionStringText;
            await Task.Delay(1000);
            _processText.gameObject.SetActive(false);
            _connectionStringPanel.SetActive(true);
            _connectCancelButton.SetActive(false);
        }

        void ActivateProgressUI()
        {
            _processText.gameObject.SetActive(true);
            _loadImage.SetActive(true);
        }
        void DeactivateProgressUI()
        {
            _processText.gameObject.SetActive(false);
            _loadImage.SetActive(false);
        }
    }
}
