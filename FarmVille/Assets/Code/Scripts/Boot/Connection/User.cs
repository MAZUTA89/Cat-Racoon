using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ClientServer;
using Assets.Code.Scripts.Lobby;
using Unity.VisualScripting;
using Assets.Code.Scripts.Boot.Data;
using Assets.Code.Scripts.Communication;
using Assets.Code.Scripts.Boot.Communication;
using TMPro;
using ClientServer.Server;
using ClientServer.Client;

namespace Assets.Code.Scripts.Boot
{
    public class User : BootSingleton<User>
    {
        const int c_tick = 00005;
        TCPBase _userBase;
        public static DateTime LoadTime;
        public static bool IsConnectionCreated { get; private set; }
        public ConnectionType ConnectionType { get; private set; }
        public PlayerType PlayerType { get; private set; }
        Communicator _communicator;
        
        private void Start()
        {
            IsConnectionCreated = false;
            LevelLoader.onLevelLoadedEvent += OnLevelLoaded;
            GameEvents.OnGameOverEvent += OnGameOver;
        }

        private void OnDisable()
        {
            GameEvents.InvokeGameOverEvent();
            LevelLoader.onLevelLoadedEvent -= OnLevelLoaded;
            GameEvents.OnGameOverEvent -= OnGameOver;
            _communicator?.Stop();

            
        }

        private void Update()
        {
            if(User.IsConnectionCreated)
            {
                switch(ConnectionType)
                {
                    case ConnectionType.Server:
                        {
                            if(!(_userBase as Server).CheckConnection())
                            {
                                GameEvents.InvokeGameOverEvent();
                            }
                            break;
                        }
                    case ConnectionType.Client:
                        {
                            if(!(_userBase as Client).CheckConnection())
                            {
                                GameEvents.InvokeGameOverEvent();
                            }
                            break;
                        }
                }
            }
        }

        public void InitializeUserBase(TCPBase userBase, ConnectionType connectionType)
        {
            _userBase = userBase;
            ConnectionType = connectionType;
            if(connectionType == ConnectionType.Server)
            {
                PlayerType = PlayerType.Player1;
            }
            else
                PlayerType = PlayerType.Player2;
        }

        public async void OnLevelLoaded()
        {
            _communicator = new Communicator
                (_userBase, c_tick);

            bool checkSignalResult = false;
            StartCommunicationSignal signal
            = new StartCommunicationSignal();
            StartCommunicationSignal recvSignal = new StartCommunicationSignal();

            signal.DateTime = DateTime.Now;
            int send_bytes = await _userBase.SendAcync(signal);

            if (send_bytes < 1)
            {
                Debug.Log(_userBase.GetLastError());
                return;
            }

            recvSignal =
            await _userBase.RecvAcync<StartCommunicationSignal>();

            if (recvSignal != null)
            {
                checkSignalResult = true;
            }
            else
            {
                Debug.Log(_userBase.GetLastError());
                return;
            }

            if (checkSignalResult)
            {
                if (signal.DateTime < recvSignal.DateTime)
                {
                    TimeSpan diff = recvSignal.DateTime - signal.DateTime;
                    await Task.Delay(diff);
                }
                DateTime loadTime = DateTime.Now;
                _communicator.Start();
                IsConnectionCreated = true;
                CommunicationEvents.InvokeCommunicationEvent();
            }
        }

        private void OnApplicationQuit()
        {
            switch (ConnectionType)
            {
                case ConnectionType.Server:
                    {
                        if ((_userBase as Server).Stop())
                        {
                            GameEvents.InvokeGameOverEvent();
                        }
                        break;
                    }
                case ConnectionType.Client:
                    {
                        if ((_userBase as Client).Stop())
                        {
                            GameEvents.InvokeGameOverEvent();
                        }
                        break;
                    }
            }
        }
        void OnGameOver()
        {
            _communicator?.Stop();
            User.IsConnectionCreated = false;
        }
    }
}
