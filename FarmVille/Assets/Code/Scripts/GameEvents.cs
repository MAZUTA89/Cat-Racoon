using System;
using UnityEngine;

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
            Debug.Log("GAME OVER!");
            OnGameOverEvent?.Invoke();
        }
    }
}
