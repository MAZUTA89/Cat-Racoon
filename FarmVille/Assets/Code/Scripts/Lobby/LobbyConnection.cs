using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClientServer.Client;
using ClientServer.Server;
using UnityEngine;
using System.Threading;
using Assets.Code.Scripts.Boot;
using ClientServer;
using System.Collections;
using TMPro;
using Assets.Code.Scripts.Lobby.Connection;
using Zenject;

namespace Assets.Code.Scripts.Lobby
{//Работа с подключением
    public class LobbyConnection : MonoBehaviour
    {
        const string c_MenuSceneName = "MenuScene";
        const string c_ConnectionText = "Waiting for connection...";
        const string c_ConnectText = "Connect...";
        const string c_SuccsessConnectionText = "Connection is established";
        const string c_ConnectionFailedText = "Connection failed";
        const string c_CanceledText = "Canceled...";

        public event Action OnCancelServerConnectionEvent;
        public event Action OnCancelClientConnectionEvent;
        public event Action OnStartCreateConnectionEvent;
        public event Action OnCreateConnectionSeccessEvent;
        public event Action OnCreateConnectionFailedEvent;
        public event Action<string> OnCreateServerEndPointEvent;
        public event Action OnCreateConnectionStringFailedEvent;
        //public event Action OnCreate

        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;

        [SerializeField] TMP_InputField inputField;
        TextMeshProUGUI _processText;
        GameObject _loadImage;
        Server _server;
        Client _client;
        CancellationTokenSource _cancellationTokenSource;
        ClientConnectionCreator _clientConnectionCreator;
        ServerConnectionCreator _serverConnectionCreator;
        Task<bool> _сonnectionTask;

        [Inject]
        public void Constructor(
            [Inject(Id = "ConnectionProgressText")] TextMeshProUGUI progressText,
            [Inject(Id = "ConnectionLoadImage")] GameObject loadImage)
        {
            _processText = progressText;
            _loadImage = loadImage;
        }

        private void Awake()
        {
            _clientConnectionCreator = new ClientConnectionCreator();
            _serverConnectionCreator = new ServerConnectionCreator();
        }

        public async void OnCreate()
        {
            //ActivateProgressUI();
            OnStartCreateConnectionEvent?.Invoke();
            //_processText.text = c_ConnectionText;

            _cancellationTokenSource
                = new CancellationTokenSource();

            _сonnectionTask = 
                _serverConnectionCreator
                .CreateServerConnection(_cancellationTokenSource.Token,
                OnCreateServerEndPointEvent);

            bool result = await _сonnectionTask;

            if (result)
            {
                _server = _serverConnectionCreator.GetServer();
                OnCreateConnectionSeccessEvent?.Invoke();
                //_processText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
                onServerConnectionCreatedEvent?.Invoke(_server);
            }
            else
            {
                _server?.Stop();
                OnCreateConnectionFailedEvent?.Invoke();
                //_processText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                return;
            }
        }

        public async void OnConnect()
        {
            //ActivateProgressUI();

            _cancellationTokenSource
                = new CancellationTokenSource();

            if(!_clientConnectionCreator.InitializeEndPoint(inputField.text))
            {
                OnCreateConnectionStringFailedEvent?.Invoke();
                return;
            }

            _сonnectionTask =
                _clientConnectionCreator
                .CreateClientConnection(_cancellationTokenSource.Token);

            OnStartCreateConnectionEvent?.Invoke();

            await Task.Delay(1000);
            //_processText.text = c_ConnectText;

            bool result = await _сonnectionTask;
            if (result)
            {
                _client = _clientConnectionCreator.GetClient();
                OnCreateConnectionSeccessEvent?.Invoke();
                //_processText.text = c_SuccsessConnectionText;
                onClientConnectionCreatedEvent?.Invoke(_client);
                await Task.Delay(1000);
                Debug.Log($"Подключился к {_client.GetRemotePoint()}");
            }
            else
            {
                _client?.Stop();
                //_processText.text = c_ConnectionFailedText;
                OnCreateConnectionFailedEvent?.Invoke();
                await Task.Delay(1000);
                Debug.Log("Подключение не удалось!");
            }
        }
        public void OnServerBack()
        {
            _server?.Stop();
            CancelConnection(OnCancelServerConnectionEvent);
        }
        public void OnClientBack()
        {
            _client?.Stop();
            CancelConnection(OnCancelClientConnectionEvent);
        }

        void CancelConnection(Action cancelConnectionEvent)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
            }
            catch (AggregateException)
            {
                //_processText.text = c_CanceledText;
            }
            //await Task.Delay(1000);
            
            cancelConnectionEvent?.Invoke();
            //DeactivateProgressUI();
        }

        //void ActivateProgressUI()
        //{
        //    _processText.gameObject.SetActive(true);
        //    _loadImage.SetActive(true);
        //}
        //void DeactivateProgressUI()
        //{
        //    _processText.gameObject.SetActive(false);
        //    _loadImage.SetActive(false);
        //}
    }
}
