using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Gameplay.Days
{
    [RequireComponent(typeof(Light))]
    public class Lamp : MonoBehaviour
    {
        Light _light;
        private void Start()
        {
            _light = GetComponent<Light>();
            GameEvents.OnDayStartEvent += OnDay;
            GameEvents.OnNightStartEvent += OnNight;
            _light.intensity = -1;
        }

        private void OnDisable()
        {
            GameEvents.OnDayStartEvent -= OnDay;
            GameEvents.OnNightStartEvent -= OnNight;
        }

        void OnDay()
        {
            _light.intensity = -1;
        }
        void OnNight()
        {
            _light.intensity = 1;
        }
    }
}
