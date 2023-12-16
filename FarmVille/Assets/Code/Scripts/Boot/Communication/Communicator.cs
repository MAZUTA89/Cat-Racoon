using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.Code.Scripts.Boot.Data;
using ClientServer;
using Newtonsoft.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class Communicator
    {
        TCPBase _user;
        public static PlayerData SendData { get; set; }
        public static PlayerData RecvData { get; set; }
        int _tick;

        Task _communicateTask;
        CancellationTokenSource _cancellationTokenSource;
        public Communicator(TCPBase user, int tick)
        {
            SendData = new PlayerData();
            RecvData = new PlayerData();
            _tick = tick;
            _user = user;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _communicateTask = Task.Factory.StartNew(CommunicateTask, _cancellationTokenSource.Token,
                _cancellationTokenSource.Token);
        }
        async void CommunicateTask(object state)
        {
            CancellationToken ct = (CancellationToken)state;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    RecvData = await CommunicateFix();
                    
                    await Task.Delay(_tick);
                }
                catch (JsonReaderException ex)
                {
                    Debug.Log(ex.Message);
                }
                catch(Exception ex)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Debug.Log(ex.Message);
                    });
                }

            }
        }

        public async Task<PlayerData> CommunicateFix()
        {

            int sendBytes = await _user.SendFixAcync(SendData);
            CommunicationEvents.InvokeSendDataEvent(sendBytes);
            if (sendBytes < 1)
            {
                GameEvents.InvokeGameOverEvent();
                Debug.Log(_user.GetLastError());
            }

            PlayerData recv = await _user.RecvFixAcync<PlayerData>();

            CommunicationEvents.InvokeRecvDataEvent(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(recv)
                    ).Length - 1
                );

            if (recv != null)
            {
                return recv;
            }
            else
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log("Recv is Null");
                });
                return default;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            SendData = null;
            RecvData = null;
        }

    }
}
