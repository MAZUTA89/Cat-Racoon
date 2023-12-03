using ClientServer.Client;
using ClientServer.Server;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public async void Create()
    {
        Server server = new Server();

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 9000);
        server.SetEndPoint(endPoint);

        if(!server.TryBindPoint())
        {
            Debug.Log(server.GetLastError());
            return;
        }

        server.Listen();

        if(!await server.TryAcceptAsync())
        {
            Debug.Log(server.GetLastError());
            return;
        }
    }
    public async void Connect()
    {
        EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
        Client client = new Client(endPoint);

        if(!await client.TryConnectAsync())
        {
            Debug.Log(client.GetLastError());
            return;
        }

    }
}
