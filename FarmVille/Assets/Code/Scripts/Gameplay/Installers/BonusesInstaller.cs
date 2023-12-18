using Assets.Code.Scripts.Gameplay.Bonuses;
using Assets.Code.Scripts.Gameplay.Installers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    public class BonusesInstaller : MonoInstaller
    {
        [Header("Extra:")]
        [SerializeField] BonusSO ExtraMoneyBonusSO;
        [SerializeField] GameObject ExtraMoneyBonusPrefab;
        [SerializeField] List<Transform> ExtraMoneySpawnPoints;
        [Space]
        [Space]
        [Space]
        [Header("Steal:")]
        [SerializeField] BonusSO StealMoneyBonusSO;
        [SerializeField] GameObject StealMoneyBonusPrefab;
        [SerializeField] List<Transform> StealMoneySpawnPoints;
        List<BonusSpawner> _bonusSpawners;
        public override void InstallBindings()
        {
            _bonusSpawners = new List<BonusSpawner>();


            BonusSpawner extraMoneyBonusSpawner =
                new BonusSpawner(ExtraMoneyBonusPrefab, ExtraMoneySpawnPoints,
                ExtraMoneyBonusSO);

            BonusSpawner stealMoneyBonusSpawner =
                new BonusSpawner(StealMoneyBonusPrefab, StealMoneySpawnPoints,
                StealMoneyBonusSO);

            _bonusSpawners.Add(extraMoneyBonusSpawner);
            _bonusSpawners.Add(stealMoneyBonusSpawner);

            Container.BindInstance(_bonusSpawners).WithId("MoneyBonusSpawners");
            Container.Bind<BonusService>().AsSingle();
        }
    }
}
