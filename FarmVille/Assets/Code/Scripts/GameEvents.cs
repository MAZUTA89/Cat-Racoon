using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Purchasing;

namespace Assets.Code.Scripts
{
    public class GameEvents
    {
        public static event Action<float> OnPickSeedEvent;

        public static void InvokePickSeedEvent(float money)
        {
            OnPickSeedEvent?.Invoke(money);
        }
    }
}
