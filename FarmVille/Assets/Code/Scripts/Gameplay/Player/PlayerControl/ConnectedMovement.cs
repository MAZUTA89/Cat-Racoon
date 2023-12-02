using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Player.PlayerControl
{
    public class ConnectedMovement : MonoBehaviour
    {
        public float MovementSpeed = 5f;
        Rigidbody2D _rb;
        PlayerData _recvPlayerData;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _recvPlayerData = User.Instance.RecvPlayerData;
        }

        public void FixedUpdate()
        {
            _rb.MovePosition(_recvPlayerData.GetPosition());
        }
    }
}
