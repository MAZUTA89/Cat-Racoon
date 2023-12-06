using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class ConnectedPlayerMovement : Movement
    {
        bool _isCommunicateStarted;

        private void OnEnable()
        {
            CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        }
        private void OnDisable()
        {
            CommunicationEvents.OnStartCommunicateEvent -= OnStartCommunicate;
        }
        public override void OnStart()
        {
            _isCommunicateStarted = false;
        }
        private void FixedUpdate()
        {
            if (_isCommunicateStarted)
            {
                NewPosition = Communicator.RecvData.GetPosition();
                CurrentPosition = RigidBody.position;
                CurrentPosition = Move(CurrentPosition, NewPosition);
                
            }
        }
        void OnStartCommunicate()
        {
            _isCommunicateStarted = true;
        }
    }
}
