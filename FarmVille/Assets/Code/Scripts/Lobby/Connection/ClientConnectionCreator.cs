using System;
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
        public bool InitializeEndPoint(string endPoint)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.104"), 9001);
            _endPoint = iPEndPoint;
            return true;
        }
        public async Task<Client> CreateClientConnection(object cancellationToken)
        {
            CancellationToken ct = (CancellationToken)cancellationToken;

            ct.ThrowIfCancellationRequested();
            
            Client client = new Client(_endPoint);
            
            Debug.Log($"Connect to : {client.EndPoint}");

            Task<bool> connectionTask = client.TryConnectAsync();
            bool result = false;
            Task waitConnectionTask = Task.Run(async () =>
            {
                while(!ct.IsCancellationRequested)
                {
                    if(await connectionTask)
                    {
                        result = true;
                        Debug.Log($"Подключился к {client.EndPoint}");
                        break;
                    }

                    await Task.Delay(1000, ct);
                }
            });

            await waitConnectionTask;

            if (result)
                return client;
            else return null;
        }
    }
}
