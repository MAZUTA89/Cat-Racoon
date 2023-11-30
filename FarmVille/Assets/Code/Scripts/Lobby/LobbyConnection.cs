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

        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;

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
            ActivateProgressUI();

            _processText.text = c_ConnectionText;

            _cancellationTokenSource
                = new CancellationTokenSource();

            _сonnectionTask = 
                _serverConnectionCreator
                .CreateServerConnection(_cancellationTokenSource.Token);

            bool result = await _сonnectionTask;

            if (result)
            {
                _server = _serverConnectionCreator.GetServer();
                _processText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
                onServerConnectionCreatedEvent?.Invoke(_server);
            }
            else
            {
                _processText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                return;
            }
        }

        public async void OnConnect()
        {
            ActivateProgressUI();

            _cancellationTokenSource
                = new CancellationTokenSource();

            _clientConnectionCreator.InitializeEndPoint("Str");

            _сonnectionTask =
                _clientConnectionCreator
                .CreateClientConnection(_cancellationTokenSource.Token);

            _processText.text = c_ConnectText;

            bool result = await _сonnectionTask;
            if (result)
            {
                _client = _clientConnectionCreator.GetClient();
                _processText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
                onClientConnectionCreatedEvent?.Invoke(_client);
                Debug.Log($"Подключился к {_client.GetRemotePoint()}");
            }
            else
            {
                _processText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                Debug.Log("Подключение не удалось!");
            }
        }

        public async Task<bool> LoadClientLevel(Client client)
        {
            //await _levelLoader.LoadLevelAsync
            //    (GameData.Instance.GameSceneName.ToString());

            LoadLevelSignal loadLevelSignal =
                new LoadLevelSignal(ConnectionType.Client);

            int send_bytes = await client.SendAcync(loadLevelSignal);
            if (send_bytes > 0)
            {
                try
                {
                    LoadLevelSignal serverSignal = await client.RecvAcync<LoadLevelSignal>();
                    if (serverSignal.ConnectionType == ConnectionType.Server)
                    {
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<bool> LoadServerLevel(Server server)
        {
            //await _levelLoader.LoadLevelAsync
            //    (GameData.Instance.GameSceneName.ToString());
            LoadLevelSignal loadLevelSignal =
                new LoadLevelSignal(ConnectionType.Server);

            int send_bytes = await server.SendAcync(loadLevelSignal);

            if (send_bytes > 0)
            {
                try
                {
                    LoadLevelSignal clientSignal = await server.RecvAcync<LoadLevelSignal>();
                    if (clientSignal.ConnectionType == ConnectionType.Client)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return false;
                }
            }
            return false;
        }

        public void OnServerBack()
        {
            CancelConnection(OnCancelServerConnectionEvent);
        }
        public void OnClientBack()
        {
            CancelConnection(OnCancelClientConnectionEvent);
        }

        async void CancelConnection(Action cancelConnectionEvent)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
            }
            catch (AggregateException)
            {
                _processText.text = c_CanceledText;
            }
            await Task.Delay(1000);
            cancelConnectionEvent?.Invoke();
            DeactivateProgressUI();
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
