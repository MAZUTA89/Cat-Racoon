using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientServer.Client;
using ClientServer;
using System.Net;

namespace Clientt
{
    public class Program
    {
        static Communicator _communicator;
        static async Task Main(string[] args)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9001);
            Client client = new Client(endPoint);

            _communicator = new Communicator(client);

            if (!await client.TryConnectAsync())
            {
                Console.WriteLine(client.GetLastError());
                client.Stop();
                return;
            }
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
            Communicator.SendData.UpdatePosition();
        }

    }
}
