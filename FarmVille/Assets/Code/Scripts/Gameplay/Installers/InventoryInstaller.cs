using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine.UI;
using UnityEngine;
using Assets.Code.Scripts.Gameplay.Inventory.UI;
using Assets.Code.Scripts.Gameplay.Inventory;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] InventoryInputService InventoryInputService;
        [Header("UI Cells:")]
        [SerializeField] List<Cell> Cells;
        [Header("UI Choosen Cells sprites:")]
        [SerializeField] List<Sprite> ChoosenCells;
        public override void InstallBindings()
        {
            BindInventoryInputService();
            BindCells();

            for (int i = 0; i < Cells.Count; i++)
            {
                Cells[i].SetChoosenSprite(ChoosenCells[i]);
            }
        }

        void BindInventoryInputService()
        {
            Container.BindInstance(InventoryInputService).AsSingle();
        }
        void BindCells()
        {
            Container.Bind<Cell>().AsTransient();
        }
    }
}
