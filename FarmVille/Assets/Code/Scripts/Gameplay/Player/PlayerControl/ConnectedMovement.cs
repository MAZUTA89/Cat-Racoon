﻿using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class ConnectedMovement : MonoBehaviour
    {
        public float MovementSpeed = 5f;
        public float _smoothTime = 1f;
        Rigidbody2D _rb;
        Vector2 _currPos;
        Transform _connectedSpawnPoint;
        Vector2 _velocity;
        [Inject]
        public void Constructor(
            [Inject(Id = "ConnectedSpawn")] Transform connectedSpawnPoint)
        {
            _connectedSpawnPoint = connectedSpawnPoint;
        }
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currPos = _connectedSpawnPoint.position;
        }

        public void FixedUpdate()
        {
            if (Communicator.RecvData != null)
            {
                Vector2 newPos = Communicator.RecvData.GetPosition();
                _currPos = Vector2.SmoothDamp(_currPos, newPos, ref _velocity,
                    _smoothTime, MovementSpeed);
                _rb.MovePosition(_currPos);
            }
        }
    }
}
