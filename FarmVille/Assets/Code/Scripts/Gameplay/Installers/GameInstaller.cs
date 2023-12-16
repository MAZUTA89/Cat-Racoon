using Assets.Code.Scripts.Boot.Data;
using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputSystem();
            BindTerritoryService();
            Container.Bind<CursorRay>().AsSingle();
            Container.Bind<UseItems>().AsSingle();
            Container.Bind<ItemCommandsHandler>().AsSingle();
            Container.Bind<PlantTerritory>().AsTransient();
            Container.Bind<UICommunicateStatistics>().AsSingle();
        }

        void BindInputSystem()
        {
            InputService inputService = new InputService();
            Container.BindInstance(inputService).AsSingle();
        }

        void BindTerritoryService()
        {
            TerritoryService territoryService = new TerritoryService();
            Container.BindInstance(territoryService).AsSingle();
        }
    }
}
