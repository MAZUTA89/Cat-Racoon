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

namespace Assets.Code.Scripts.Lobby
{//Работа с подключением
    public class LobbyConnection : MonoBehaviour
    {
        Server _server;
        Client _client;
        CancellationTokenSource _cancellToken;

        public async Task<bool> CreateServerConnection(CancellationToken cancellationToken)
        {
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
                _server.Stop();
                Debug.Log("Canceled in register!");
            });
            await _server.TryAcceptAsync();

            Debug.Log($"Подключение от {_server.GetRemotePoint()}");

            return true;
        }
        public async Task<bool> CreateClientConnection(CancellationToken cancellationToken)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
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

            Debug.Log($"Подключаюсь к {_client.EndPoint}");
            return true;
        }

        public void OnCreate()
        {
            Task.Run(async () =>
            {
                _cancellToken = new CancellationTokenSource();
                if (await CreateServerConnection(_cancellToken.Token))
                {
                    Debug.Log("Клиент подключился!");
                }
                else
                {
                    Debug.Log("Ошибка ожидания!");
                }
            });
        }
        public void OnConnect()
        {
            Task.Run(async () =>
            {
                _cancellToken = new CancellationTokenSource();
                await CreateClientConnection(_cancellToken.Token);
                Debug.Log("Подключение к серверу успешно!");
            });
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
