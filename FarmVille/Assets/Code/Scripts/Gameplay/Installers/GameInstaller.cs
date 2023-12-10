using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputSystem();
        }

        void BindInputSystem()
        {
            InputService inputService = new InputService();
            Container.BindInstance(inputService).AsSingle();
            Container.Bind<CursorRay>().AsSingle();
            Container.Bind<UseItems>().AsSingle();
            Container.Bind<ItemCommandsHandler>().AsSingle();
        }
    }
}
