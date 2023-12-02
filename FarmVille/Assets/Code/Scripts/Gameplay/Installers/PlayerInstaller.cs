using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMovement();
        }
        void BindMovement()
        {
            Container.Bind<TopDownMovement>().AsSingle();
        }
    }
}
