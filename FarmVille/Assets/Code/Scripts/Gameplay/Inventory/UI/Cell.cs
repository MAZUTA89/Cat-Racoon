using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Inventory.UI
{
    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour
    {
        [Range(1, 7)]
        public int Number;
        Image _image;
        Sprite _defaultSprite;
        Sprite _choosenSprite;
        InventoryInputService _inventoryInputService;
        [Inject]
        public void Constructor(InventoryInputService inventoryInputService)
        {
            _inventoryInputService = inventoryInputService;
        }
        private void OnEnable()
        {
            _inventoryInputService.OnChooseCellNumberEvent += OnChooseCell;
        }
        private void OnDisable()
        {
            _inventoryInputService.OnChooseCellNumberEvent -= OnChooseCell;
        }
        private void Start()
        {
            _image = GetComponent<Image>();
            _defaultSprite = _image.sprite;
        }

        public void SetChoosenSprite(Sprite sprite)
        {
            _choosenSprite = sprite;
        }

        void OnChooseCell(int number)
        {
            if(Number == number)
                _image.sprite = _choosenSprite;
            else
                _image.sprite = _defaultSprite;
        }
    }
}
