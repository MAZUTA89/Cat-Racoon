using ClientServer.Server;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby
{
    public class ServerConnectionCreator
    {
        Server _server;
        public async Task<bool> CreateServerConnection
            (CancellationToken cancellationToken,
            Action<string> onCreateServerEndpoint)
        {
            bool result = false;
            _server?.Stop();
            _server = new Server();
            if (!_server.TryBindPoint())
            {
                Debug.Log(_server.GetLastError());

                return _server.Stop();
            }
            onCreateServerEndpoint?.Invoke(_server.GetLocalPoint());
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
            if(result)
                Debug.Log($"Подключение от {_server.GetRemotePoint()}");

            return result;
        }

        public Server GetServer()
        {
            return _server;
        }
    }
}
