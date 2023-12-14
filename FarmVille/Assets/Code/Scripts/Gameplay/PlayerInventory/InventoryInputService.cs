using System;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    public class InventoryInputService : MonoBehaviour
    {
        InputService _inputService;
        public event Action<int> OnChooseCellNumberEvent;
        public event Action<Item> OnChooseCellTypeEvent;
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }

        public void Update()
        {
            if (_inputService.IsCell_1())
            {
                OnChooseCellTypeEvent?.Invoke(Item.Basket);
                OnChooseCellNumberEvent?.Invoke(1);
            }
            if (_inputService.IsCell_2())
            {
                OnChooseCellTypeEvent?.Invoke(Item.Watering);
                OnChooseCellNumberEvent?.Invoke(2);
            }
            if (_inputService.IsCell_3())
            {
                OnChooseCellTypeEvent?.Invoke(Item.Wheat);
                OnChooseCellNumberEvent?.Invoke(3);
            }
            if (_inputService.IsCell_4())
            {
                OnChooseCellTypeEvent?.Invoke(Item.SunFlower);
                OnChooseCellNumberEvent?.Invoke(4);
            }
            if (_inputService.IsCell_5())
            {
                OnChooseCellTypeEvent?.Invoke(Item.Pumpking);
                OnChooseCellNumberEvent?.Invoke(5);
            }
            if (_inputService.IsCell_6())
            {
                OnChooseCellTypeEvent?.Invoke(Item.Reed);
                OnChooseCellNumberEvent?.Invoke(6);
            }
            if (_inputService.IsCell_7())
            {
                OnChooseCellTypeEvent?.Invoke(Item.InfernalGrowth);
                OnChooseCellNumberEvent?.Invoke(7);
            }
        }
    }
}
