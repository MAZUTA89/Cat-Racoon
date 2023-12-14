using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;


namespace Assets.Code.Scripts.Gameplay
{
    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour
    {
        public Item Item;
        Image _image;
        Sprite _defaultSprite;
        Sprite _choosenSprite;
        InventoryInputService _inventoryInputService;
        public TextMeshProUGUI Text;
        Inventory _inventory;
        [Inject]
        public void Constructor(InventoryInputService inventoryInputService, Inventory inventory)
        {
            _inventoryInputService = inventoryInputService;
            _inventory = inventory;
        }
        private void OnEnable()
        {
            _inventoryInputService.OnChooseCellTypeEvent += OnChooseCell;
        }
        private void OnDisable()
        {
            _inventoryInputService.OnChooseCellTypeEvent -= OnChooseCell;
        }
        private void Start()
        {
            _image = GetComponent<Image>();
            _defaultSprite = _image.sprite;
        }
        private void Update()
        {
            if(Item != Item.Basket)
            {
                Text.text = _inventory[Item].ToString();
            }
        }
        public void SetChoosenSprite(Sprite sprite)
        {
            _choosenSprite = sprite;
        }

       
        void OnChooseCell(Item cellType)
        {
            if (Item == cellType)
                _image.sprite = _choosenSprite;
            else
                _image.sprite = _defaultSprite;
        }
    }
}
