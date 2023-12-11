using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts
{
    public class GameEvents
    {
        public static event Action<float> OnPickSeedEvent;
        public static event Action OnGameOverEvent;
        public static event Action OnDayStartEvent;
        public static event Action OnNightStartEvent;

        public static void InvokePickSeedEvent(float money)
        {
            OnPickSeedEvent?.Invoke(money);
        }
        public static void InvokeOnDayStartEvent(float intensivity)
        {
            OnDayStartEvent?.Invoke();
        }
        public static void InvokeOnNightStartEvent(float intensivity)
        {
            OnNightStartEvent?.Invoke();
        }
        public static void InvokeGameOverEvent()
        {
            OnGameOverEvent?.Invoke();
        }
    }
}
