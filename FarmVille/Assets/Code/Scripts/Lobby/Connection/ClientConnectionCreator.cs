﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientServer.Client;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby.Connection
{
    public class ClientConnectionCreator
    {
        EndPoint _endPoint;
        Client _client;
        public bool InitializeEndPoint(string endPoint)
        {
            IPEndPoint iPEndPoint = 
                new IPEndPoint(IPAddress.Parse("192.168.0.104"), 9001);
            _endPoint = iPEndPoint;
            return true;
        }
        public async Task<bool> CreateClientConnection
            (CancellationToken cancellationToken)
        {
            CancellationToken ct = cancellationToken;

            ct.ThrowIfCancellationRequested();

            _client = new Client(_endPoint);

            Debug.Log($"Connect to : {_client.EndPoint}");

            bool result = false;

            ct.Register(() =>
            {
                _client.Stop();
            });

            Task waitConnectionTask = Task.Run(async () =>
            {
                while (true)
                {
                    if (ct.IsCancellationRequested == true)
                    {
                        result = false;
                        break;
                    }
                    if (await _client.TryConnectAsync())
                    {
                        result = true;
                        Debug.Log($"Подключился к {_client.EndPoint}");
                        break;
                    }

                    await Task.Delay(1000);
                }
            }, ct);

            await waitConnectionTask;

            return result;
        }

        public Client GetClient()
        {
            return _client;
        }
    }
}
