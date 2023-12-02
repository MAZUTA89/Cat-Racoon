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

        protected override void OnAwake()
        {
            SendPlayerData = new PlayerData();
            RecvPlayerData = new PlayerData();
            ConnectionType = ConnectionType.Server;
            _communicator = new Communicator
                (_userBase, SendPlayerData, RecvPlayerData, 1000);
        }
        private void Start()
        {
            LevelLoader.onLevelLoadedEvent += OnLevelLoaded;
        }

        private void OnDisable()
        {
            LevelLoader.onLevelLoadedEvent -= OnLevelLoaded;
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
            bool checkSignalResult = false;
            Task checkCommunicationSignalTask =
                Task.Run(async () =>
                {
                    StartCommunicationSignal signal
                    = new StartCommunicationSignal();

                    int send_bytes = await _userBase.SendAcync(signal);

                    if (send_bytes < 1)
                    {
                        checkSignalResult = false;
                        return;
                    }

                    var recvSignal =
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

            if (checkSignalResult)
            {
                // Получаем текущее время
                LoadTime = DateTime.Now;
            }
            else
            {
                Debug.Log("Не удалось проверить сигнал!");
                return;
            }

            // Преобразуем время в строку и выводим в консоль
            Debug.Log("Current Time: " + LoadTime.ToString());
        }
    }
}
