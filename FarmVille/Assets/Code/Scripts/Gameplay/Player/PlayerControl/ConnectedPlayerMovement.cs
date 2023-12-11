using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if(input != Vector2.zero)
                    Debug.Log($"Recv input: {input}");
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
