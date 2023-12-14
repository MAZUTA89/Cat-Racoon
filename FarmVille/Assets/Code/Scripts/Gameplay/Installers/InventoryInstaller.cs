using System.Collections.Generic;
using Zenject;
using UnityEngine;


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
            BindInventory();

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
        void BindInventory()
        {
            Inventory inventory = new Inventory();
            Container.BindInstance(inventory).AsSingle();
        }
    }
}
