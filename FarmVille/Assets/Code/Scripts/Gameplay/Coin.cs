using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay
{
    public class Coin : MonoBehaviour
    {
        public TextMeshProUGUI CoinText;
        float _currentMoney;

        private void Start()
        {
            _currentMoney = 0;
            GameEvents.OnPickSeedEvent += OnPickSeed;
        }
        private void OnDisable()
        {
            GameEvents.OnPickSeedEvent -= OnPickSeed;
        }
        private void Update()
        {
            CoinText.text = _currentMoney.ToString();
        }
        public void OnPickSeed(float money)
        {
            _currentMoney += money;
        }

    }
}
