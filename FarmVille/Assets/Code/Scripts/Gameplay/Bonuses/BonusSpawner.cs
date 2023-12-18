using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Scripts.Gameplay.Bonuses
{
    public class BonusSpawner
    {
        public event Action<GameObject, Transform> OnSpawnBonus;
        GameObject _prefab;
        List<Transform> _spawnPoints;
        List<Transform> _emptyPoints;
        BonusSO _bonusSO;
        float _currentTime;
        public BonusSpawner(GameObject prefab, List<Transform> spawnPoints, BonusSO bonusSO)
        {
            _prefab = prefab;
            _spawnPoints = spawnPoints;
            _bonusSO = bonusSO;
            _emptyPoints = new List<Transform>(_spawnPoints);

        }

        public void Tick(float time)
        {
            _currentTime += time;
            if (_currentTime >= _bonusSO.PeriodTime)
            {
                int randChance = Random.Range(1, 121);
                if (randChance < _bonusSO.Chance)
                {
                    Debug.Log($"Spawn {_prefab.name}!!!");
                    OnSpawnBonus?.Invoke(_prefab, )
                }
                _currentTime = 0;
            }
        }
        public void OnPickUpBonus()
        {

        }
        public bool GetEmptyPoint(out Transform emptyPoint)
        {
            if (_emptyPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, _emptyPoints.Count);
                emptyPoint = _emptyPoints[randomIndex];
                _emptyPoints.RemoveAt(randomIndex);
                return true;
            }
            else
            {
                emptyPoint = null;
                return false;
            }
        }
    }
}
