using ClientServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby
{
    public class ServerConnectionCreator
    {
        Server _server;
        public async Task<bool> CreateServerConnection
            (CancellationToken cancellationToken)
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
