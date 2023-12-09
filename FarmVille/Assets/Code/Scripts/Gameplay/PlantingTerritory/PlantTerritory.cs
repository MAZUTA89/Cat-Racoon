using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.PlantingTerritory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantTerritory : MonoBehaviour
    {
        public bool IsEmpty { get; protected set; }
        [SerializeField] List<Item> SeedsToGrow;
        protected SpriteRenderer Sprite;
        Color _defaultColor;
        private void Start()
        {
            IsEmpty = true;
            Sprite = GetComponent<SpriteRenderer>();
            _defaultColor = Sprite.color;
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

        public void SetEmpty(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }

    }
}
