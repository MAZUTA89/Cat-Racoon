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
        Timer _timer;
        TCPBase _tcpBase;
        int _tick;
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
            
            CommunicatorArgs args = new CommunicatorArgs();
            args.Token = tokenSource.Token;
            args.TCPBase = _tcpBase;
            Task.Factory.StartNew(Communicate, args);
        }

        
        public async void Communicate(object state)
        {
            CommunicatorArgs args = (CommunicatorArgs)state;
            CancellationToken ct = args.Token;
            TCPBase tcpBase = args.TCPBase;
            while (!ct.IsCancellationRequested)
            {

                Debug.Log("Tick");


                //Debug.Log($"Error {tcpBase.GetLastError()}");
                int send_bytes = await tcpBase.SendAcync(_sendData);
                if (send_bytes < 1)
                {
                    Debug.Log(tcpBase.GetLastError());
                    return;
                }

                Debug.Log($"Send: {send_bytes}");
                PlayerData recvData = await tcpBase.RecvAcync<PlayerData>();
                if (recvData != null)
                {
                    _recvData = recvData;
                    Debug.Log($"Recv pos: {_recvData.GetPosition()}");
                }
                else
                {
                    Debug.Log(tcpBase.GetLastError());
                    Debug.Log("Ошибка получения данных!");
                    return;
                }
                await Task.Delay(_tick);
            }
        }

        public void Stop()
        {
            tokenSource?.Cancel();
        }
    }
}
