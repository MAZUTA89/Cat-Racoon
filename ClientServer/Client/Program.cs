using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClientServer;
using ClientServer.Client;

namespace Clientt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Адресс:");
            string ip = Console.ReadLine();
            Console.WriteLine("Порт:");
            int port = int.Parse(Console.ReadLine());
            
            IPEndPoint iPEndPoint;
            Client client;

            if (Assistant.TryParseEndPoint(ip, (ulong)port, out iPEndPoint))
            {
                client = new Client(iPEndPoint);
            }
            else return;

            if(await client.TryConnectAsync())
            {
                Console.WriteLine("подключено!");
            }
            else
            {
                Console.WriteLine("Connection failed!");
                Console.WriteLine("Error:" + client.GetLastError());
            }

            client.TryStop();

        }
    }
}
