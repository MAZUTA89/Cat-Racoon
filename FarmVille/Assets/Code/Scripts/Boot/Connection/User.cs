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

namespace Assets.Code.Scripts.Boot
{
    public class User : BootSingleton<User>
    {
        TCPBase _userBase;
        public static DateTime LoadTime;
        public PlayerData SendPlayerData;
        public PlayerData RecvPlayerData;
        public ConnectionType ConnectionType { get; private set; }
        Communicator _communicator;
        public TextMeshProUGUI RecvTimeText;
        public TextMeshProUGUI SendTimeText;
        public TextMeshProUGUI DiffTimeText;
        public TextMeshProUGUI StartTimeText;

        protected override void OnAwake()
        {
            SendPlayerData = new PlayerData();
            RecvPlayerData = new PlayerData();
            ConnectionType = ConnectionType.Server;
            
        }
        private void Start()
        {
            LevelLoader.onLevelLoadedEvent += OnLevelLoaded;
        }

        private void OnDisable()
        {
            LevelLoader.onLevelLoadedEvent -= OnLevelLoaded;
            _communicator?.Stop();
        }

        private void Update()
        {

        }

        public void InitializeUserBase(TCPBase userBase, ConnectionType connectionType)
        {
            _userBase = userBase;
            ConnectionType = connectionType;
        }

        public async void OnLevelLoaded()
        {
            _communicator = new Communicator
                (_userBase, SendPlayerData, RecvPlayerData, 50);

            bool checkSignalResult = false;
            StartCommunicationSignal signal
            = new StartCommunicationSignal();
            StartCommunicationSignal recvSignal = new StartCommunicationSignal();
            Task checkCommunicationSignalTask =
                Task.Run(async () =>
                {
                    signal.DateTime = DateTime.Now;
                    int send_bytes = await _userBase.SendAcync(signal);

                    if (send_bytes < 1)
                    {
                        checkSignalResult = false;
                        return;
                    }

                    recvSignal =
                    await _userBase.RecvAcync<StartCommunicationSignal>();

                    if (recvSignal != null)
                    {
                        checkSignalResult = true;
                        return;
                    }
                    else
                    {
                        checkSignalResult = false;
                        return;
                    }

                });

            await checkCommunicationSignalTask;

            SendTimeText.text += signal.DateTime.ToString() + " " + signal.DateTime.Millisecond;
            RecvTimeText.text += recvSignal.DateTime.ToString() + " " + recvSignal.DateTime.Millisecond;
            if(checkSignalResult)
            {
                if(signal.DateTime < recvSignal.DateTime)
                {
                    TimeSpan diff = recvSignal.DateTime - signal.DateTime;
                    DiffTimeText.text += diff.ToString();
                    await Task.Delay(diff);
                }
                DateTime loadTime = DateTime.Now;
                StartTimeText.text += loadTime.ToString() + " " + loadTime.Millisecond;
                _communicator.Start();
            }

            // Преобразуем время в строку и выводим в консоль
            Debug.Log("Current Time: " + LoadTime.ToString());
        }
    }
}
