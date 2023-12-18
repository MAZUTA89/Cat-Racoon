using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;

namespace Assets.Code.Scripts.Gameplay
{
    public class Coin : MoneyDisplayer
    {
        public override float CurrentMoney => GameData.Instance.PlayerMoney;
    }
}
