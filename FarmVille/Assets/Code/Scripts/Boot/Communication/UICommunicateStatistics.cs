using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Assets.Code.Scripts.Boot.Communication;
using Zenject;

namespace Assets.Code.Scripts.Boot.Data
{
    public class UICommunicateStatistics : MonoBehaviour
    {
        [SerializeField] GameObject StatisticsPanel;
        [SerializeField] TextMeshProUGUI SendSizeText;
        [SerializeField] TextMeshProUGUI RecvSizeText;

        InputService _inputService;
        bool _isOpen;
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            _isOpen = false;
            StatisticsPanel.SetActive(false);
            CommunicationEvents.OnSendDataEvent += OnSendData;
            CommunicationEvents.OnRecvDataEvent += OnRecvData;
        }

        private void OnDisable()
        {
            CommunicationEvents.OnSendDataEvent -= OnSendData;
            CommunicationEvents.OnRecvDataEvent -= OnRecvData;
        }

        private void Update()
        {
            if(_inputService.IsStatsCommand())
            {
                _isOpen = !_isOpen;
                StatisticsPanel.SetActive(_isOpen);
            }

            
        }

        public void OnSendData(int bytes)
        {
            SendSizeText.text = bytes.ToString();
        }
        public void OnRecvData(int bytes)
        {
            RecvSizeText.text = bytes.ToString();
        }
    }
}
