using ClientServer;
using ClientServer.Client;
using ClientServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby.LoadingLevel
{
    public class LevelSignalChecker
    {
        const float c_waitLoadLevelSignalTime = 5f;
        Task<int> _sendLoadLevelSignalTask;
        Task<LoadLevelSignal> _recvLoadLevelSignalTask;
        Task<Task> _returnComplitedTask;
        bool _result;
        
        public async Task<bool> CheckServerLevelSignal(Server server,
            CancellationToken ct)
        {
            _result = false;
            ct.Register(() =>
            {
                ct.ThrowIfCancellationRequested();
                server.Stop();
            });

            LoadLevelSignal loadLevelSignal
                = new LoadLevelSignal(ConnectionType.Server);

            InitializeLevelLoadCompliteTask(
                loadLevelSignal,
                server);

            int sendBytes = await _sendLoadLevelSignalTask;

            if (sendBytes < 1)
            {
                return false;
            }

            _result = await WaitForSignalOrReciveSignal(ConnectionType.Client);

            return _result;
        }

        public async Task<bool> CheckClientLevelSignal(Client client,
            CancellationToken ct)
        {
            _result = false;
            ct.Register(() =>
            {
                ct.ThrowIfCancellationRequested();
                client.Stop();
            });

            LoadLevelSignal loadLevelSignal
                = new LoadLevelSignal(ConnectionType.Client);

            InitializeLevelLoadCompliteTask(
                loadLevelSignal,
                client);

            int sendBytes = await _sendLoadLevelSignalTask;
            //return false; 
            ///если нужно протестировать случай, когда 
            ///клиент не отправил сигнал
            if (sendBytes < 1)
            {
                return false;
            }
            _result = await WaitForSignalOrReciveSignal(ConnectionType.Server);
            
            return _result;
        }

        void InitializeLevelLoadCompliteTask(LoadLevelSignal loadLevelSignal,
            TCPBase user)
        {
            _sendLoadLevelSignalTask = user.SendAcync(loadLevelSignal);
            _recvLoadLevelSignalTask = user.RecvAcync<LoadLevelSignal>();

            _returnComplitedTask = Task.WhenAny(
                _recvLoadLevelSignalTask,
                Task.Delay(TimeSpan.FromSeconds(c_waitLoadLevelSignalTime)
                ));
        }

        async Task<bool> WaitForSignalOrReciveSignal(ConnectionType connectionType)
        {
            bool result = false;

            Task complitedTask = await _returnComplitedTask;

            switch (complitedTask)
            {
                case Task<LoadLevelSignal> recvTask:
                    {
                        LoadLevelSignal levelSignal = recvTask.Result;

                        if (levelSignal != null &
                            levelSignal.ConnectionType == connectionType)
                        {
                            result = true;
                        }
                        else
                            result = false;
                        break;
                    }
                default:
                    {
                        result = false;
                        break;
                    }
            }
            return result;
        }
    }
}
