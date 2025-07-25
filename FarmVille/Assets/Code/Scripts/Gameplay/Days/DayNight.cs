﻿using UnityEngine;
using TMPro;

namespace Assets.Code.Scripts.Gameplay
{
    public class DayNight : MonoBehaviour
    {
        public TextMeshProUGUI DaysText;
        public float DayIntensivity;
        public float NightIntensivity;
        [SerializeField] Light Lighting;
        public int DaysToEnd;
        public float DayTime;
        public float TestDayTime;
        public static int CurrentDays;
        public float _currentTime;
        public float AddIntensivity;
        [SerializeField] bool IsTest;
        bool _flag;
        bool _isGameOver;
        private void Start()
        {
            _flag = true;
            _currentTime = 0f;
            _isGameOver = false;
            if(IsTest)
            {
                DayTime = TestDayTime;
            }
        }

        private void Update()
        {
            if(CurrentDays >= DaysToEnd && _isGameOver == false)
            {
                _isGameOver = true;
                CurrentDays = 0;
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
                Lighting.intensity = Mathf.Lerp(Lighting.intensity, DayIntensivity, AddIntensivity * Time.deltaTime);
            }
            else
            {
                Lighting.intensity = Mathf.Lerp(Lighting.intensity, NightIntensivity, AddIntensivity * Time.deltaTime);
            }

            DaysText.text = CurrentDays.ToString();
        }
    }
}
