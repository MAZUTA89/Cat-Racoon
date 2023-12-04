using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using ClientServer;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Communication
{
    public class Communicator
    {
        PlayerData _sendData;
        PlayerData _recvData;
        TCPBase _tcpBase;
        int _tick;
        Timer _time;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public Communicator(TCPBase user, PlayerData sendData, PlayerData recvData, int tick)
        {
            _recvData = recvData;
            _sendData = sendData;
            _tick = tick;
            _tcpBase = user;
        }

        public void Start()
        {
            _time = new Timer(TimerCallBack, null, 0, _tick);
            SendRecvData();
        }

        void TimerCallBack(object state)
        {
            SendRecvData();
        }
        async void SendRecvData()
        {
            CommunicatorArgs args = new CommunicatorArgs();
            args.Token = tokenSource.Token;
            args.TCPBase = _tcpBase;
            args.SendData = _sendData;
            Task<PlayerData> task = await Task.Factory.StartNew(Communicate, args);

            _recvData = task.Result;
        }
        public async Task<PlayerData> Communicate(object state)
        {
            CommunicatorArgs args = (CommunicatorArgs)state;
            TCPBase tcpBase = args.TCPBase;
            Debug.Log("Tick");
            if (_sendData.HasChanges)
            {
                int send_bytes = await tcpBase.SendAcync(args.SendData);
                if (send_bytes < 1)
                {
                    Debug.Log(tcpBase.GetLastError());
                    return default;
                }
            }
            //Debug.Log($"Send: {send_bytes}");
            PlayerData recvData = await tcpBase.RecvAcync<PlayerData>();
            if (recvData != null)
            {
                User.Instance.RecvPlayerData = recvData;
                //Debug.Log($"Recv pos: {recvData.GetPosition()}");
                return recvData;
            }
            else
            {
                Debug.Log(tcpBase.GetLastError());
                Debug.Log("Ошибка получения данных!");
                return default;
            }
        }

        public void Stop()
        {
            tokenSource?.Cancel();
            _time.Dispose();
        }
        public PlayerData GetData()
        {
            return _recvData;
        }
    }
}
