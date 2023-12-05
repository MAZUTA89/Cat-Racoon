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
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.104"), 9001);
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
            await Task.Delay(30);
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
