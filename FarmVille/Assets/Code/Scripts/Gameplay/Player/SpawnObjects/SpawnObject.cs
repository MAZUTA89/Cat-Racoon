﻿using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Player.SpawnObject
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField] GameObject Object;
        bool _isCommunicateStarted;
        InputService _inputService;
        private void OnEnable()
        {
            CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        }
        private void OnDisable()
        {
            CommunicationEvents.OnStartCommunicateEvent -= OnStartCommunicate;
        }
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }
        private void Update()
        {
            if (_isCommunicateStarted)
            {
                if (_inputService.IsSpawn())
                {
                    if(Object.activeSelf)
                    {
                        Object.SetActive(false);
                    }
                    else
                    {
                        Object.SetActive(true);
                    }
                    Communicator.SendData.IsSpawning = true;
                }
                else
                {
                    Communicator.SendData.IsSpawning = false;
                }
            }
        }

        void OnStartCommunicate()
        {
            _isCommunicateStarted = true;
        }
    }
}
