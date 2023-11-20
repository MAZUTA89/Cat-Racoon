using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClientServer;
using ClientServer.Client;
using ClientServer.Server;

namespace Clientt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Адресс:");
            ////string ip = Console.ReadLine();
            //Console.WriteLine("Порт:");
            //int port = int.Parse(Console.ReadLine());
            
            IPEndPoint iPEndPoint;
            Client client;

            if (Assistant.TryParseEndPoint("127.0.0.1", 9000, out iPEndPoint))
            {
                client = new Client(iPEndPoint);
            }
            else return;

            Task con = client.TryConnectAsync();

            //if(await client.TryConnectAsync()))
            //{
            //    Console.WriteLine("подключено!");
            //}
            //else
            //{
            //    Console.WriteLine("Connection failed!");
            //    Console.WriteLine("Error:" + client.GetLastError());
            //}
            await con;
            Console.WriteLine("Подключено!");
            Task sendTask = Send(client);
            Task recvTask = Recv(client);

            Task.WaitAny(recvTask, sendTask);

            client.TryStop();
            Console.ReadLine();
        }

        static async Task Send(Client client)
        {
            int i = 0;
            while (true)
            {
                client.SendAsync(i.ToString());
                await Task.Delay(1000);
                i++;
            }
        }
        static async Task Recv(Client client)
        {
            while (true)
            {
                string str = await client.RecvAsync();
                Console.WriteLine($"Take: {str}");
            }
        }
    }
}
