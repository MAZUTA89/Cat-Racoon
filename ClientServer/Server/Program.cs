using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            await Print();
            Console.WriteLine($"Ожидание подключения на {server.EndPoint}");
            if (!await server.TryAcceptAsync())
            {
                Console.WriteLine("Accept failed");
                server.TryStop();
            }
            else Console.WriteLine("Подключено!");
        }

        static async Task Print()
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Print");
                await Task.Delay(500);
            }
        }
    }
}
