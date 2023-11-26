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
        static Person Person { get; set; }
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Адресс:");
            ////string ip = Console.ReadLine();
            //Console.WriteLine("Порт:");
            //int port = int.Parse(Console.ReadLine());
            
            
            IPEndPoint iPEndPoint;
            Client client;

            if (Assistant.TryParseEndPoint("192.168.0.104", 9000, out iPEndPoint))
            {
                client = new Client(iPEndPoint);
            }
            else return;

            Task con =  client.TryConnectAsync();

            await con;
            Console.WriteLine("Подключено!");

            Person = await client.RecvAcync<Person>();
            //Task sendTask = Send(client);
            //Task recvTask = Recv(client);

            //Task.WaitAny(recvTask, sendTask);

            client.Stop();
            Console.WriteLine($"Person {Person.Name}");
            Console.ReadLine();
        }
    }
}
