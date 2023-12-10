using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientServer.Server;
using ClientServer;
using System.Net;

namespace Serverr
{
    internal class Program
    {
        static Communicator _communicator;
        static async Task Main(string[] args)
        {
            Server server = new Server();
            _communicator = new Communicator(server);

            server.SetEndPoint(new IPEndPoint(IPAddress.Any, 9001));

            if (!server.TryBindPoint())
            {
                server.Stop();
                Console.WriteLine(server.GetLastError());
                return;
            }
            server.Listen();

            if (!await server.TryAcceptAsync())
            {
                server.Stop();
                Console.WriteLine(server.GetLastError());
                return;
            }

            Console.WriteLine($"Подключение: {server.GetRemotePoint()}");

            Task updateTaks = Task.Run(() =>
            {
                while (true)
                {
                    Update();
                }
            });

            _communicator.Start();

            updateTaks.Wait();

            _communicator.Stop();
        }

        public static void Update()
        {
            Communicator.SendData.UpdateItem();
            Communicator.SendData.UpdatePosition();
        }

    }
}
