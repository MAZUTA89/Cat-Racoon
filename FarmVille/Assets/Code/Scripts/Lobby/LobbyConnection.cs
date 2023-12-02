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
        public event Action OnCancelServerConnectionEvent;
        public event Action OnCancelClientConnectionEvent;
        public event Action OnStartCreateConnectionEvent;
        public event Action OnCreateConnectionSeccessEvent;
        public event Action OnCreateConnectionFailedEvent;
        public event Action<string> OnCreateServerEndPointEvent;
        public event Action OnCreateConnectionStringFailedEvent;

        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;

        [SerializeField] TMP_InputField inputField;
        
        Server _server;
        Client _client;
        CancellationTokenSource _cancellationTokenSource;
        ClientConnectionCreator _clientConnectionCreator;
        ServerConnectionCreator _serverConnectionCreator;
        Task<bool> _сonnectionTask;

        private void Awake()
        {
            _clientConnectionCreator = new ClientConnectionCreator();
            _serverConnectionCreator = new ServerConnectionCreator();
        }

        public async void OnCreate()
        {
            OnStartCreateConnectionEvent?.Invoke();

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
                await Task.Delay(1000);
                onServerConnectionCreatedEvent?.Invoke(_server);
            }
            else
            {
                _server?.Stop();
                OnCreateConnectionFailedEvent?.Invoke();
                await Task.Delay(1000);
                return;
            }
        }

        public async void OnConnect()
        {
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

            bool result = await _сonnectionTask;
            if (result)
            {
                _client = _clientConnectionCreator.GetClient();
                OnCreateConnectionSeccessEvent?.Invoke();
                onClientConnectionCreatedEvent?.Invoke(_client);
                await Task.Delay(1000);
                Debug.Log($"Подключился к {_client.GetRemotePoint()}");
            }
            else
            {
                _client?.Stop();
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
            }
            cancelConnectionEvent?.Invoke();
        }
    }
}
