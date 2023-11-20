using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientServer.Client;
using ClientServer.Server;

namespace Serverr
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server();

            if (!server.TryBindPoint())
            {
                Console.WriteLine("Ошибка bind");
                return;
            }

            if (!server.Listen())
            {
                Console.WriteLine("Ошибка listen");
                return;
            }
            Console.WriteLine($"Ожидание подключения на {server.EndPoint}");

            Task t = server.TryAcceptAsync();
            
            await t;
            Console.WriteLine("Подключено!");
            Task sendTask = Send(server);
            Task recvTask = Recv(server);

            Task.WaitAny(sendTask, recvTask);

            server.TryStop();
        }
        static async Task Send(Server server)
        {
            int i = 0;
            while (true)
            {
                server.SendAsync(i.ToString());
                Task.Delay(1000);
                i++;
            }
        }
        static async Task Recv(Server server)
        {
            while (true)
            {
                Console.WriteLine($"Take: {server.RecvAsync()}");
            }
        }
    }
}
