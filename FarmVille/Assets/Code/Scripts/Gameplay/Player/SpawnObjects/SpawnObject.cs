using Assets.Code.Scripts.Boot.Communication;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay.Player.SpawnObject
{
    public class SpawnObject : MonoBehaviour
    {
        [SerializeField] GameObject Object;
        bool _isCommunicateStarted;
        InputService _inputService;
        private void OnEnable()
        {
            CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        }
        private void OnDisable()
        {
            CommunicationEvents.OnStartCommunicateEvent -= OnStartCommunicate;
        }
        [Inject]
        public void Constructor(InputService inputService)
        {
            _inputService = inputService;
        }
        private void Update()
        {
            if (_isCommunicateStarted)
            {
                if (_inputService.IsSpawn())
                {
                    
                }
                else
                {
                }
            }
        }
        void OnStartCommunicate()
        {
            _isCommunicateStarted = true;
        }
    }
}
