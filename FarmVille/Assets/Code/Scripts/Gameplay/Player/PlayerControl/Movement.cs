using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Movement : MonoBehaviour
    {
        protected Rigidbody2D RigidBody;
        protected float MovementSpeed = 3f;
        protected float _defaultSpeed;
        protected float _smoothTime = 2f;
        protected float BoostSpeed = 10f;
        Vector2 _velocity;
        protected Vector2 CurrentPosition;
        protected Vector2 NewPosition;
        protected Animator Animator;
        protected int XKey;
        protected int YKey;

        private void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            XKey = Animator.StringToHash("X");
            YKey = Animator.StringToHash("Y");
            _defaultSpeed = MovementSpeed;
            OnStart();
        }

        protected void InputAndAnimateInFouthDirections(ref Vector2 input)
        {
            if (input.x > 0)
            {
                Animator.SetFloat(XKey, 1);
                Animator.SetFloat(YKey, 0);
                input.y = 0;
            }
            else if (input.x < 0)
            {
                Animator.SetFloat(XKey, -1);
                Animator.SetFloat(YKey, 0);
                input.y = 0;
            }
            else if (input.y > 0)
            {
                Animator.SetFloat(YKey, 1);
                Animator.SetFloat(XKey, 0);
                input.x = 0;
            }
            else
            {
                Animator.SetFloat(YKey, -1);
                Animator.SetFloat(XKey, 0);
                input.x = 0;
            }

            if (input.x == 0 && input.y == 0)
            {
                Animator.SetFloat(YKey, 0);
                Animator.SetFloat(XKey, 0);
            }
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
