using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;

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
