using TMPro;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay
{
    public abstract class MoneyDisplayer : MonoBehaviour
    {
        public TextMeshProUGUI CoinText;
        public abstract float CurrentMoney { get; }
        
        private void Update()
        {
            CoinText.text = CurrentMoney.ToString();
        }
        public float GetMoney()
        {
            return CurrentMoney;
        }
    }
}
