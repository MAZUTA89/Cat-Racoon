using Assets.Code.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class SeedsInstaller : MonoInstaller
    {
        public List<SeedSO> SeedsSO;
        public List<Seed> SeedsPrefabs;
        Dictionary<Item, Seed> _seedsGO;
        Dictionary<Item, SeedSO> _seedsSO;
        public override void InstallBindings()
        {
            //SeedsService seedsService =

            InitSeedsGO();
            InitSeedsSO();

            SeedsService seedsService = new SeedsService(_seedsGO, _seedsSO);

            Container.BindInstance(seedsService).AsSingle();
        }

        void InitSeedsGO()
        {
            _seedsGO = new Dictionary<Item, Seed>();

            foreach (var seed in SeedsPrefabs)
            {
                _seedsGO[seed.GetSeedType()] = seed;
            }
        }
        void InitSeedsSO()
        {
            _seedsSO = new Dictionary<Item, SeedSO>();

            foreach (var seedSO in SeedsSO)
            {
                _seedsSO[seedSO.SeedType] = seedSO;
            }
        }
    }
}
