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

namespace Assets.Code.Scripts.Lobby
{//Работа с подключением
    public class LobbyConnection : MonoBehaviour
    {
        const string c_MenuSceneName = "MenuScene";
        public event Action<Server> onServerConnectionCreatedEvent;
        public event Action<Client> onClientConnectionCreatedEvent;
        Server _server;
        Client _client;
        CancellationTokenSource _cancellToken;
        LevelLoader _levelLoader;

        private void Awake()
        {
            _levelLoader = new LevelLoader();
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

            await Task.Run(async () =>
            {
                while (true)
                {
                    if (await _client.TryConnectAsync())
                    {
                        return;
                    }
                }
            });

            Debug.Log($"Подключился к {_client.EndPoint}");
            return true;
        }

        public async void OnCreate()
        {
            _cancellToken = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellToken.Token;
            bool result;
            _server = new Server();
            if (!_server.TryBindPoint())
            {
                Debug.Log(_server.GetLastError());

                result = _server.Stop();
            }

            if (!_server.Listen())
            {
                Debug.Log(_server.GetLastError());
                result = false;
            }

            Debug.Log($"Слушаю на {_server.EndPoint}");

            cancellationToken.Register(() =>
            {
                result = false;
                _server.Stop();
                Debug.Log("Canceled in register!");
            });

            result = await _server.TryAcceptAsync();

            if (result)
            {
                Debug.Log("Клиент подключился!");
            }
            else
            {
                _server.Stop();
                Debug.Log("Ошибка ожидания!");
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
            _cancellToken = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellToken.Token;
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.104"), 9001);
            _client = new Client(iPEndPoint);

            cancellationToken.Register(() =>
            {
                _client.Stop();
                Debug.Log("Client stop connection!");
            });

            Debug.Log($"Connect to : {_client.EndPoint}");

            if (await _client.TryConnectAsync())
            {
                Debug.Log($"Подключился к {_client.EndPoint}");
            }
            else
            {
                Debug.Log("Ошибка подкючения!");
            }

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
        }
        public void OnClientBack()
        {
            _cancellToken?.Cancel();
        }
    }
}
