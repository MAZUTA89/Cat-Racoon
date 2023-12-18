using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Bonuses
{
    public abstract class Bonus : MonoBehaviour
    {
        const string c_playerTag = "Player";
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == c_playerTag)
            {
                Destroy(gameObject);
            }
        }
    }
}
