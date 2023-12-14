using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class ConnectedPlayerMovement : Movement
    {
        public override void OnStart()
        {
        }

        private void Update()
        {
            if(User.IsConnectionCreated)
            {
                Vector2 input = Communicator.RecvData.GetDirection();
                
                InputAndAnimateInFouthDirections(ref input);
            }
        }

        private void FixedUpdate()
        {
            if (User.IsConnectionCreated)
            {
                NewPosition = Communicator.RecvData.GetPosition();
                CurrentPosition = RigidBody.position;
                CurrentPosition = Move(CurrentPosition, NewPosition);
                
            }
        }
        
    }
}
