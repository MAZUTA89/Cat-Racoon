using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Coins
{
    public class ConnectedCoin : MoneyDisplayer
    {
        private void Update()
        {
            if(User.IsConnectionCreated)
            {
                CoinText.text = Communicator.RecvData.Money.ToString();
            }
        }
        protected override void Disable()
        {
        }

        protected override void OnStart()
        {
        }
    }
}
