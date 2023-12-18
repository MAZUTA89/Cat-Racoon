using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;

namespace Assets.Code.Scripts.Gameplay.Coins
{
    public class ConnectedCoin : MoneyDisplayer
    {
        public override float CurrentMoney => GameData.Instance.ConnectedPlayerMoney;

    }
}
