using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ClientServer;
using Assets.Code.Scripts.Lobby;

namespace Assets.Code.Scripts.Boot
{
    public class User : BootSingleton<User>
    {
        TCPBase _userBase;
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

        public void InitializeUserBase(TCPBase userBase)
        {
            _userBase = userBase;
        }

        public void OnLevelLoaded()
        {
            // Получаем текущее время
            DateTime currentTime = DateTime.Now;

            // Преобразуем время в строку и выводим в консоль
            Debug.Log("Current Time: " + currentTime.ToString());
        }
    }
}
