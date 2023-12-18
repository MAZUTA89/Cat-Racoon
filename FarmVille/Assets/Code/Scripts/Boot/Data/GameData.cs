
namespace Assets.Code.Scripts.Boot
{
    public class GameData : BootSingleton<GameData>
    {
        public SceneName GameSceneName = SceneName.Farm;

        public float PlayerMoney;
        public float ConnectedPlayerMoney;

        public void SetSceneName(SceneName sceneName)
        {
            GameSceneName = sceneName;
        }
    }
}
