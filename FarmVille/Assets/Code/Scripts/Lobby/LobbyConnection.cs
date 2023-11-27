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

        public TextMeshProUGUI ProcessText;
        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;
        Server _server;
        Client _client;
        CancellationTokenSource _cancellationTokenSource;
        LevelLoader _levelLoader;
        ClientConnectionCreator _clientConnectionCreator;

        Task<bool> _сonnectionTask;

        private void Awake()
        {
            _levelLoader = new LevelLoader();
            _clientConnectionCreator = new ClientConnectionCreator();
        }

        public async Task<bool> CreateServerConnection(CancellationToken cancellationToken)
        {
            bool result = false;
            _server = new Server();
            if (!_server.TryBindPoint())
            {
                Debug.Log(_server.GetLastError());

                return _server.Stop();
            }

            if (!_server.Listen())
            {
                Debug.Log(_server.GetLastError());
                return false;
            }

            Debug.Log($"Слушаю на {_server.EndPoint}");

            cancellationToken.Register(() =>
            {
                result = false;
                _server.Stop();
                Debug.Log("Canceled in register!");
                cancellationToken.ThrowIfCancellationRequested();
            });

            result = await _server.TryAcceptAsync();

            //Debug.Log($"Подключение от {_server.GetRemotePoint()}");

            return result;
        }


        public async void OnCreate()
        {
            ProcessText.gameObject.SetActive(true);

            ProcessText.text = c_ConnectionText;

            _cancellationTokenSource
                = new CancellationTokenSource();

            _сonnectionTask = CreateServerConnection(_cancellationTokenSource.Token);

            bool result = await _сonnectionTask;

            if (result)
            {
                ProcessText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
            }
            else
            {
                ProcessText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                ProcessText.gameObject.SetActive(false);
                return;
            }
        }

        public async void OnConnect()
        {
            ProcessText.gameObject.SetActive(true);

            _cancellationTokenSource
                = new CancellationTokenSource();

            _clientConnectionCreator.InitializeEndPoint("Str");

            _сonnectionTask =
                _clientConnectionCreator.CreateClientConnection(_cancellationTokenSource.Token);

            ProcessText.text = c_ConnectText;

            bool result = await _сonnectionTask;
            if (result)
            {
                _client = _clientConnectionCreator.GetClient();
                ProcessText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
                Debug.Log($"Подключился к {_client.GetRemotePoint()}");
            }
            else
            {
                ProcessText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                Debug.Log("Подключение не удалось!");
            }

            ProcessText.gameObject.SetActive(false);
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

        public async void OnServerBack()
        {
            CancelConnection();
            await Task.Delay(1000);
            OnCancelServerConnectionEvent?.Invoke();
            ProcessText.gameObject.SetActive(false);
        }
        public async void OnClientBack()
        {
            CancelConnection();
            await Task.Delay(1000);
            OnCancelClientConnectionEvent?.Invoke();
            ProcessText.gameObject.SetActive(false);
        }

        void CancelConnection()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
            }
            catch (AggregateException)
            {
                ProcessText.text = c_CanceledText;
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }

    }
}
