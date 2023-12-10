using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Code.Scripts.Gameplay
{
    public class CursorRay : MonoBehaviour
    {
        Camera _camera;
        InputService _inputService;
        public RaycastHit2D Hit;
        public string HitObject;
        public event Action<RaycastHit2D> OnHitEvent;
        [Inject]
        public void Construcotr(InputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_inputService.IsMouseLeftButton())
            {
                Ray ray = _camera.ScreenPointToRay(_inputService.GetMousePosition());

                Hit = Physics2D.GetRayIntersection(ray);

                if (Hit.collider != null)
                {
                    OnHitEvent?.Invoke(Hit);
                    HitObject = Hit.collider.gameObject.name;
                }
            }
            HandleCommunicationData();
        }

        void HandleCommunicationData()
        {
            if (User.IsConnectionCreated)
            {
                if (_inputService.IsMouseLeftButton())
                {
                    Communicator.SendData.IsLeftButton = true;
                }
                else
                {
                    Communicator.SendData.IsLeftButton = false;
                }
            }
        }
    }
}
