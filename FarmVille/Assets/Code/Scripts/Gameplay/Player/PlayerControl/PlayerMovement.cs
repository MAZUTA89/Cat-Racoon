using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using Assets.Code.Scripts.Boot.Communication;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class PlayerMovement : Movement
    {
        InputService _inputService;
        Vector2 _input;
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }
        public override void OnStart()
        {
        }

        protected virtual void Update()
        {
            _input = _inputService.GetMovement();
        }

        private void FixedUpdate()
        {
            NewPosition = RigidBody.position + _input;
            Communicator.SendData?.UpdatePosition(NewPosition);
            CurrentPosition = RigidBody.position;
            CurrentPosition = Move(CurrentPosition, NewPosition);
        }
    }
}
