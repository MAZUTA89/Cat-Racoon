using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using TMPro;

namespace Assets.Code.Scripts.Gameplay
{
    public class DayNight : MonoBehaviour
    {
        public TextMeshProUGUI DaysText;
        public float DayIntensivity;
        public float NightIntensivity;
        [SerializeField] Light Lighting;
        List<Light> SpotLights;
        public int DaysToEnd;
        public float DayTime;
        public float TestDayTime;
        public static int CurrentDays;
        public float _currentTime;
        public float AddIntensivity;
        [SerializeField] bool IsTest;
        bool _flag;
        [Inject]
        public void Constructor(List<Light> lights)
        {
            SpotLights = lights;
        }
        private void Start()
        {
            _flag = true;
            _currentTime = 0f;
            if(IsTest)
            {
                DayTime = TestDayTime;
            }
        }

        private void Update()
        {
            if(CurrentDays >= DaysToEnd)
            {
                GameEvents.InvokeGameOverEvent();
                return;
            }
            _currentTime += Time.deltaTime;
            if (_currentTime >= DayTime)
            {
                if(_flag == false)
                {
                    GameEvents.InvokeOnDayStartEvent(DayIntensivity);
                    CurrentDays++;
                }
                else
                {
                    GameEvents.InvokeOnNightStartEvent(NightIntensivity);
                }

                _flag = !_flag;
                _currentTime = 0;
            }

            if (_flag)
            {
                DeactivateSpotLights();
                Lighting.intensity = Mathf.Lerp(Lighting.intensity, DayIntensivity, AddIntensivity * Time.deltaTime);
            }
            else
            {
                ActivateSpotLights();
                Lighting.intensity = Mathf.Lerp(Lighting.intensity, NightIntensivity, AddIntensivity * Time.deltaTime);
            }

            DaysText.text = CurrentDays.ToString();
        }

        public void StartNight()
        {
            Task.Run(async () =>
            {
                while (Lighting.intensity >= NightIntensivity)
                {
                    Lighting.intensity -= AddIntensivity;
                    await Task.Delay(10);
                }
                CurrentDays++;
            });
        }

        void ActivateSpotLights()
        {
            foreach (var light in SpotLights)
            {
                light.gameObject.SetActive(true);
            }
        }
        void DeactivateSpotLights()
        {
            foreach (var light in SpotLights)
            {
                light.gameObject.SetActive(false);
            }
        }
    }
}
