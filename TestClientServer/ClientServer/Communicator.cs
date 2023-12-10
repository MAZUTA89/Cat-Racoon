using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServer
{
    public class Communicator
    {
        TCPBase _user;
        public static PlayerData SendData { get; set; }
        public static PlayerData RecvData { get; set; }

        Timer _timer;
        public Communicator(TCPBase user) 
        {
            SendData = new PlayerData();
            RecvData = new PlayerData();
            _user = user;
        }

        public void Start()
        {
            _timer = new Timer(TimerCallBack, 0, 0, 100);
        }

        async void TimerCallBack(object state)
        {
            try
            {
                //RecvData = await Communicate();
                RecvData = await CommunicateFix();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<PlayerData> Communicate()
        {
            int sendBytes = await _user.SendAcync(SendData);
            if(sendBytes < 1)
            {
                Console.WriteLine(_user.GetLastError());
            }

            PlayerData recv = await _user.RecvAcync<PlayerData>();

            if (recv != null)
            {
                return recv;
            }
            else
            {
                Console.WriteLine(_user.GetLastError());
                return default;
            }
        }
        public async Task<PlayerData> CommunicateFix()
        {
            int sendBytes = await _user.SendFixAcync(SendData);
            if (sendBytes < 1)
            {
                Console.WriteLine(_user.GetLastError());
            }

            PlayerData recv = await _user.RecvFixAcync<PlayerData>();

            if (recv != null)
            {
                PrintPlayerData(recv);
                return recv;
            }
            else
            {
                Console.WriteLine("Recv is Null");
                return default;
            }
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        void PrintPlayerData(PlayerData playerData)
        {
            Console.WriteLine($"recv x: {playerData.PositionX} y: {playerData.PositionY}");
            List<Item> items = playerData.GetItemType();
            foreach (Item item in items)
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
        }
    }
}
