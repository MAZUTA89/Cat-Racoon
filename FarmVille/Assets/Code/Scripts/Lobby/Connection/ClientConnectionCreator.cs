using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientServer.Client;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Assets.Code.Scripts.Lobby.Connection
{
    public class ClientConnectionCreator
    {
        const string c_endPointPattern
            = @"^(?'IP'\d+\.\d+\.\d+\.\d+):(?'Port'\d+)$";
        Regex _endPointRegex;
        EndPoint _endPoint;
        Client _client;
        public ClientConnectionCreator()
        {
            _endPointRegex = new Regex(c_endPointPattern);
        }
        public bool InitializeEndPoint(string endPoint)
        {
            int port;
            string ip;
            if (_endPointRegex.IsMatch(endPoint))
            {
                Match match = _endPointRegex.Match(endPoint);
                ip = match.Groups["IP"].Value;
                if (!int.TryParse(match.Groups["Port"].Value, out port))
                {
                    return false;
                }
                else
                {
                    IPAddress iPAddress;
                    if(!IPAddress.TryParse(ip, out iPAddress))
                    {
                        return false;
                    }
                    else
                    {
                        if (port > IPEndPoint.MinPort && port < IPEndPoint.MaxPort)
                        {
                            _endPoint = new IPEndPoint(iPAddress, port);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
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
