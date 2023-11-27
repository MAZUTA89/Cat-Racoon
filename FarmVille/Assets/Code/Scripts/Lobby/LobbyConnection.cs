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


        public TextMeshProUGUI ProcessText;
        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;
        Server _server;
        Client _client;
        CancellationTokenSource _cancellToken;
        LevelLoader _levelLoader;
        ClientConnectionCreator _clientConnectionCreator;

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
            });

            result = await _server.TryAcceptAsync();

            //Debug.Log($"Подключение от {_server.GetRemotePoint()}");

            return result;
        }
        public async Task<bool> CreateClientConnection(CancellationToken cancellationToken)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.104"), 9001);
            _client = new Client(iPEndPoint);

            cancellationToken.Register(() =>
            {
                _client.Stop();
                Debug.Log("Client stop connection!");
            });

            Debug.Log($"Connect to : {_client.EndPoint}");

            Task<bool> connectionTask = _client.TryConnectAsync();

            while (true)
            {
                if (await connectionTask)
                {
                    break;
                }
            }

            Debug.Log($"Подключился к {_client.EndPoint}");
            return true;
        }

        public async void OnCreate()
        {
            ProcessText.gameObject.SetActive(true);

            CancellationTokenSource cancellationTokenSource
                = new CancellationTokenSource();

            ProcessText.text = c_ConnectionText;

            Task<bool> serverConnectionTask
                = CreateServerConnection(cancellationTokenSource.Token);
            bool result = await serverConnectionTask;

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

            //if (await CreateServerConnection(_cancellToken.Token))
            //{
            //    onServerConnectionCreatedEvent?.Invoke(_server);
            //    Debug.Log("Клиент подключился!");
            //}
            //else
            //{
            //    _server.Stop();
            //    Debug.Log("Ошибка ожидания!");
            //    return;
            //}
        }

        public async void OnConnect()
        {
            ProcessText.gameObject.SetActive(true);

            CancellationTokenSource cancellationTokenSource
                = new CancellationTokenSource();

            _clientConnectionCreator.InitializeEndPoint("Str");

            Task<Client> connectTask =
                _clientConnectionCreator.CreateClientConnection(cancellationTokenSource.Token);

            ProcessText.text = c_ConnectText;

            Client client = await connectTask;
            if(client == null)
            {
                ProcessText.text = c_ConnectionFailedText;
                await Task.Delay(1000);
                Debug.Log("Подключение не удалось!");
            }
            else
            {
                ProcessText.text = c_SuccsessConnectionText;
                await Task.Delay(1000);
                Debug.Log($"Подключился к {client.GetRemotePoint()}");
            }

            ProcessText.gameObject.SetActive(false);
            




            //Task<bool> clientConnectionTask 
            //    = CreateClientConnection(cancellationTokenSource.Token);

            //bool result = await clientConnectionTask;

            //if(result)
            //{
            //    ProcessText.text = c_SuccsessConnectionText;
            //    await Task.Delay(1000);
            //}
            //else
            //{
            //    ProcessText.text = c_ConnectionFailedText;
            //    await Task.Delay(1000);
            //    return;
            //}


            //await Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        if (await _client.TryConnectAsync())
            //        {
            //            return;
            //        }
            //    }
            //});


            //if (await CreateClientConnection(_cancellToken.Token))
            //{
            //    onClientConnectionCreatedEvent?.Invoke(_client);
            //    Debug.Log("Подключение к серверу успешно!");
            //}
            //else
            //{
            //    Debug.Log("Ошибка подкючения!");
            //    _client.Stop();
            //    return;
            //}
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
            _cancellToken?.Cancel();
            ProcessText.gameObject.SetActive(false);
        }
        public void OnClientBack()
        {
            _cancellToken?.Cancel();
            ProcessText.gameObject.SetActive(false);
        }
    }
}
