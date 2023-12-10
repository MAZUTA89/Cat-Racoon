using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay
{
    public class Coin : MoneyDisplayer
    {
        private void Update()
        {
            CoinText.text = CurrentMoney.ToString();
        }
        public void OnPickSeed(float money)
        {
            CurrentMoney += money;
            if(IsConnectionCreated)
            {
                Communicator.SendData.Money = CurrentMoney;
            }
        }

        protected override void OnStart()
        {
            GameEvents.OnPickSeedEvent += OnPickSeed;
        }

        protected override void Disable()
        {
            GameEvents.OnPickSeedEvent -= OnPickSeed;
        }
    }
}
