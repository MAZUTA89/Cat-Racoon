using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Code.Scripts.Gameplay.PlantingTerritory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantTerritory : MonoBehaviour
    {
        public bool IsEmpty { get; protected set; }
        [SerializeField] List<Item> SeedsToGrow;
        protected SpriteRenderer Sprite;
        Color _defaultColor;
        TerritoryService _territoryService;
        Seed _seed;
        [Inject]
        public void Constructor(TerritoryService territoryService)
        {
            _territoryService = territoryService;
        }
        private void Start()
        {
            IsEmpty = true;
            Sprite = GetComponent<SpriteRenderer>();
            _defaultColor = Sprite.color;
            _territoryService.AddTerritory(this);
            RandomlyFlip();
        }
        void RandomlyFlip()
        {
            // Генерируем случайное булево значение для flipX
            bool randomFlipX = Random.value > 0.5f;

            // Генерируем случайное булево значение для flipY
            bool randomFlipY = Random.value > 0.5f;

            // Устанавливаем свойства flipX и flipY
            Sprite.flipX = randomFlipX;
            Sprite.flipY = randomFlipY;
        }
        void OnMouseEnter()
        {
            Sprite.color = new Color(Sprite.color.r, Sprite.color.g,
                Sprite.color.b, Sprite.color.a / 2);
        }
        void OnMouseExit()
        {
            Sprite.color = _defaultColor;
        }
        public bool IsTerritoryContain(Item seed)
        {
            return SeedsToGrow.Contains(seed);
        }
        public void SetSeed(Seed seed)
        {
            _seed = seed;
        }
        public void DestroySeed()
        {
            if(_seed != null)
            {
                Destroy(_seed.gameObject);
            }
        }
        public void SetEmpty(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }

    }
}
