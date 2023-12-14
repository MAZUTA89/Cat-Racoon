using TMPro;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay
{
    public abstract class MoneyDisplayer : MonoBehaviour
    {
        public TextMeshProUGUI CoinText;
        protected float CurrentMoney;

        private void Start()
        {
            CurrentMoney = 0;
            OnStart();
        }

        private void OnDisable()
        {
            Disable();
        }
        protected abstract void OnStart();
        protected abstract void Disable();

        
        public float GetMoney()
        {
            return CurrentMoney;
        }
    }
}
