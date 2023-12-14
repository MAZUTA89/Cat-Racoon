using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;

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
            if(User.IsConnectionCreated)
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
