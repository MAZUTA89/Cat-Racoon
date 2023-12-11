using Assets.Code.Scripts.Gameplay.PlantingTerritory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Seed : MonoBehaviour
    {
        [SerializeField] Item Type;
        protected float GrowingTime;
        protected Sprite GrowingUpSprite;
        public float Money { get; private set; }
        SpriteRenderer _sprite;
        Color _defaultColor;
        public GrowStatus Status { get; private set; }
        float _currentTime;

        PlantTerritory _parent;

        private void Start()
        {
            _currentTime = 0;
            Status = GrowStatus.Growing;
            _sprite = GetComponent<SpriteRenderer>();
            _defaultColor = _sprite.color;
        }

        private void Update()
        {
            if (Status == GrowStatus.Growing)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime >= GrowingTime)
                {
                    Status = GrowStatus.Ready;
                    _currentTime = 0;
                    _sprite.sprite = GrowingUpSprite;
                }
            }
        }

        public void Initialize(SeedSO seedSO, PlantTerritory parent)
        {
            GrowingTime = seedSO.GrowingTime;
            GrowingUpSprite = seedSO.GrowingUpSprite;
            Money = seedSO.Money;
            _parent = parent;
        }
        public void Initialize(SeedSO seedSO)
        {
            GrowingTime = seedSO.GrowingTime;
            GrowingUpSprite = seedSO.GrowingUpSprite;
            Money = seedSO.Money;
        }

        public void Boost()
        {
            _currentTime += 1f;
        }
        public Item GetSeedType()
        {
            return Type;
        }
        public void OnPick()
        {
            _parent?.SetEmpty(true);
        }

        public string GetParentTerritoryName()
        {
            return _parent.name;
        }
        private void OnMouseEnter()
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g,
                _sprite.color.b, _sprite.color.a / 2);
        }
        private void OnMouseExit()
        {
            _sprite.color = _defaultColor;
        }
    }
}
