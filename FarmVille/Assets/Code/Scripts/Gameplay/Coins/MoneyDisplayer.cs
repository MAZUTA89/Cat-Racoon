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
    public abstract class MoneyDisplayer : MonoBehaviour
    {
        public TextMeshProUGUI CoinText;
        protected float CurrentMoney;
        protected bool IsConnectionCreated;

        private void Start()
        {
            CurrentMoney = 0;
            IsConnectionCreated = false;
            CommunicationEvents.OnStartCommunicateEvent += OnConnectionCreated;
            OnStart();
        }

        private void OnDisable()
        {
            CommunicationEvents.OnStartCommunicateEvent -= OnConnectionCreated;
            Disable();
        }
        protected abstract void OnStart();
        protected abstract void Disable();

        void OnConnectionCreated()
        {
            IsConnectionCreated = true;
        }

    }
}
