using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.Code.Scripts.Boot.Data;
using ClientServer;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class Communicator
    {
        TCPBase _user;
        public static PlayerData SendData { get; set; }
        public static PlayerData RecvData { get; set; }
        int _tick;

        Timer _timer;
        public Communicator(TCPBase user, int tick) 
        {
            SendData = new PlayerData();
            RecvData = new PlayerData();
            _tick = tick;
            _user = user;
        }

        public void Start()
        {
            _timer = new Timer(TimerCallBack, 0, 0, _tick);
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

    }
}
