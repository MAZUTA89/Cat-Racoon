using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Inventory
{
    public class InventoryInputService : MonoBehaviour
    {
        InputService _inputService;
        public event Action<int> OnChooseCellNumberEvent;
        public event Action<CellType> OnChooseCellTypeEvent;
        public event Action<Seed> OnChooseSeedEvent;
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }

        public void Update()
        {
            if (_inputService.IsCell_1())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.Basket);
                OnChooseCellNumberEvent?.Invoke(1);
            }
            if (_inputService.IsCell_2())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.Watering);
                OnChooseCellNumberEvent?.Invoke(2);
            }
            if (_inputService.IsCell_3())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.Wheat);
                OnChooseSeedEvent?.Invoke(Seed.Wheat);
                OnChooseCellNumberEvent?.Invoke(3);
            }
            if (_inputService.IsCell_4())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.SunFlower);
                OnChooseSeedEvent?.Invoke(Seed.SunFlower);
                OnChooseCellNumberEvent?.Invoke(4);
            }
            if (_inputService.IsCell_5())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.Pumpking);
                OnChooseSeedEvent?.Invoke(Seed.Pumpking);
                OnChooseCellNumberEvent?.Invoke(5);
            }
            if (_inputService.IsCell_6())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.Reed);
                OnChooseSeedEvent?.Invoke(Seed.Reed);
                OnChooseCellNumberEvent?.Invoke(6);
            }
            if (_inputService.IsCell_7())
            {
                OnChooseCellTypeEvent?.Invoke(CellType.InfernalGrowth);
                OnChooseSeedEvent?.Invoke(Seed.InfernalGrowth);
                OnChooseCellNumberEvent?.Invoke(7);
            }
        }


    }
}
