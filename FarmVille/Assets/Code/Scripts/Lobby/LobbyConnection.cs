using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClientServer.Client;
using ClientServer.Server;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby
{//Работа с подключением
    public class LobbyConnection : MonoBehaviour
    {
        public async Task<bool> CreateServerConnection()
        {
            Server server = new Server();
            if (!server.TryBindPoint())
            {
                Debug.Log(server.GetLastError());
                return false;
            }

            if (!server.Listen())
            {
                Debug.Log(server.GetLastError());
                return false;
            }
           
            
            await server.TryAcceptAsync();

            return true;
        }
        public async Task<bool> CreateClientConnection()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
            Client client = new Client(iPEndPoint);

             client.TryConnectAsync();


            Debug.Log($"Подключаюсь к {client.EndPoint}");
            return true;
        }

        public void OnCreate()
        {
            CreateServerConnection();
            Debug.Log($"Слушаю");
        }
        public void OnConnect()
        {
            CreateClientConnection();
        }
    }
}
