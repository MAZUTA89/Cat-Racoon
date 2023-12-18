using Zenject;
using UnityEngine;
using Assets.Code.Scripts.Boot.Communication;
using Assets.Code.Scripts.Boot;
using Newtonsoft.Json.Bson;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class PlayerMovement : Movement
    {
        InputService _inputService;
        Vector2 _input;
        float _boostSpeed = 5f;
        float _boostSpeedTime = 3f;
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }
        public override void OnStart()
        {
            GameEvents.OnSpeedUpEvent += OnSpeedUp;
        }

        protected virtual void Update()
        {
            _input = _inputService.GetMovement();
            if (User.IsConnectionCreated)
            {
                Communicator.SendData.SetDirection(_input);
                Communicator.SendData.MovementSpeed = MovementSpeed;
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

        void OnSpeedUp()
        {
            MovementSpeed = _boostSpeed;
            Invoke("OnEndBoost", _boostSpeedTime);
        }

        void OnEndBoost()
        {
            MovementSpeed = _defaultSpeed;
        }
        
    }
}
