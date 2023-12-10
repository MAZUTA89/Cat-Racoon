using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
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
        public float DayTime = 30f;
        public static int Days;
        public float _currentTime;
        public float AddIntensivity;
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
        }

        private void Update()
        {
            _currentTime += Time.fixedDeltaTime;
            if (_currentTime >= DayTime)
            {
                if(_flag == false)
                {
                    GameEvents.InvokeOnDayStartEvent(DayIntensivity);
                    Days++;
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

            DaysText.text = Days.ToString();
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
                Days++;
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
