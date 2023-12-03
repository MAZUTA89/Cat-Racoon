using Assets.Code.Scripts.Boot.Data;
using ClientServer;
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
        float _tick;
        public Communicator(TCPBase user, PlayerData sendData, PlayerData recvData, float tick)
        {
            _recvData = recvData;
            _sendData = sendData;
            _tick = tick;
            _tcpBase = user;
        }

        public void Start()
        {
            _timer = new Timer(TimerCallBack, null,
                TimeSpan.Zero, TimeSpan.FromMilliseconds(_tick));
        }

        void TimerCallBack(object sender)
        {
            Debug.Log("Tick");
            Task.Run(async () =>
            {
                int send_bytes = await _tcpBase.SendAcync(_sendData);
                if(send_bytes < 1)
                {
                    Debug.Log(_tcpBase.GetLastError());
                    return;
                }

                Debug.Log($"Send: {send_bytes}");
                return;
            });

            Task.Run(async () =>
            {
                PlayerData recvData = await _tcpBase.RecvAcync<PlayerData>();
                if(recvData != null)
                {
                    _recvData = recvData;
                    Debug.Log($"Recv pos: {_recvData.GetPosition()}");
                    return;
                }
                else
                {
                    Debug.Log(_tcpBase.GetLastError());
                    Debug.Log("Ошибка получения данных!");
                    return;
                }
                
            });
        }

        
    }
}
