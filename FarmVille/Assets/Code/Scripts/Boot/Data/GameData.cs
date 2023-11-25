using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Boot
{
    public class GameData : BootSingleton<GameData>
    {
        public SceneName GameSceneName = SceneName.Farm;

        public void SetSceneName(SceneName sceneName)
        {
            GameSceneName = sceneName;
        }
    }
}
