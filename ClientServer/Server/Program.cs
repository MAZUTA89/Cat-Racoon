using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientServer;
using ClientServer.Client;
using ClientServer.Server;
using static System.Net.Mime.MediaTypeNames;

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

            Person person = new Person()
            {
                Name = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
                Age = 200,
                SecondName = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
            };
            

            await server.SendAcync(person);
            //Task sendTask = Send(server);
            //Task recvTask = Recv(server);

            //Task.WaitAny(sendTask, recvTask);

            server.Stop();
        }
    }
}
