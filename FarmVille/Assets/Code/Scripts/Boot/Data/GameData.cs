
using Assets.Code.Scripts.Boot.Communication;

namespace Assets.Code.Scripts.Boot
{
    public class GameData : BootSingleton<GameData>
    {
        public SceneName GameSceneName = SceneName.Farm;
        public float PlayerMoney {  get; set; }
        public float ConnectedPlayerMoney {  get; set; }
        public void Update()
        {
            if(User.IsConnectionCreated)
            {
                Communicator.SendData.Money = PlayerMoney;
                ConnectedPlayerMoney = Communicator.RecvData.Money;
            }
        }
        public void AddMoney(float money)
        {
            PlayerMoney += money;
            if (PlayerMoney < 0)
            {
                PlayerMoney = 0;
                return;
            }
        }
        public void StealMoney(float money)
        {
            PlayerMoney -= money;
            if(PlayerMoney < 0)
            {
                PlayerMoney = 0;
                return;
            }
        }
        public void SetSceneName(SceneName sceneName)
        {
            GameSceneName = sceneName;
        }
    }
}
