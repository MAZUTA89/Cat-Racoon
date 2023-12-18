using Assets.Code.Scripts.Boot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Bonuses
{
    public class Bonus : MonoBehaviour
    {
        const string c_playerTag = "Player";
        public event EventHandler OnPickUpBonus;
        public Transform ParentSpawnPoint;
        [SerializeField] protected float MoneyValue;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == c_playerTag)
            {
                GameData.Instance.AddMoney(MoneyValue);
                OnPickUpBonus?.Invoke(ParentSpawnPoint, new EventArgs());
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }
}
