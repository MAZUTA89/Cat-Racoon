using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Movement : MonoBehaviour
    {
        protected Rigidbody2D RigidBody;
        protected float MovementSpeed = 5f;
        protected float _smoothTime = 2f;
        Vector2 _velocity;
        protected Vector2 CurrentPosition;
        protected Vector2 NewPosition;

        private void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            OnStart();
        }

        protected Vector2 Move(Vector2 currPos, Vector2 newPos)
        {
            newPos = Vector2.SmoothDamp(currPos, newPos, ref _velocity,
                Time.fixedDeltaTime, MovementSpeed);
            RigidBody.MovePosition(newPos);
            return RigidBody.position;
        }
        public abstract void OnStart();
    }
}
