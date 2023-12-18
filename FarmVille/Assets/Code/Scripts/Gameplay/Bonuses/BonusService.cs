using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Gameplay.Installers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Bonuses
{
    public class BonusService : MonoBehaviour
    {
        List<BonusSpawner> _bonusSpawners;
        PlayerMode _mode;
        [Inject]
        public void Constructor(
            [Inject(Id = "MoneyBonusSpawners")] List<BonusSpawner> bonusSpawner,
            PlayerMode mode)
        {
            _bonusSpawners = bonusSpawner;
            _mode = mode;
        }

        private void Start()
        {
            if(_mode == PlayerMode.Multiple)
            {
                if (User.IsConnectionCreated == false)
                {
                    enabled = false;
                }
                else
                {
                    if (User.Instance.ConnectionType == ConnectionType.Client)
                    {
                        enabled = false;
                    }
                }
            }
        }
        private void Update()
        {
            foreach (var spawner in _bonusSpawners)
            {
                spawner.Tick(Time.deltaTime);
            }
        }
    }
}
