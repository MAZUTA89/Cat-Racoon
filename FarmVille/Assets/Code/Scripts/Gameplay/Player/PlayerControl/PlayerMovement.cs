using Zenject;
using UnityEngine;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot;

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
            if (User.IsConnectionCreated)
            {
                Communicator.SendData.SetDirection(_input);
               
            }
            InputAndAnimateInFouthDirections(ref _input);

        }

        private void FixedUpdate()
        {
            NewPosition = RigidBody.position + _input;
            if(User.IsConnectionCreated)
                Communicator.SendData?.UpdatePosition(NewPosition);
            CurrentPosition = RigidBody.position;
            CurrentPosition = Move(CurrentPosition, NewPosition);
        }
    }
}
