using Assets.Code.Scripts.Boot;
using Assets.Code.Scripts.Boot.Communication;
using System;
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
        public event Action<RaycastHit2D> OnHitTerritoryEvent;
        public event Action<RaycastHit2D> OnHitSeedEvent;
        public LayerMask SeedMask;
        public LayerMask TerritoryMask;
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

                CheckIntersection(ray, TerritoryMask, OnHitTerritoryEvent);
                CheckIntersection(ray, SeedMask, OnHitSeedEvent);
                
                HandleCommunicationData();
            }
            void CheckIntersection(Ray ray, LayerMask mask, Action<RaycastHit2D> hitEvent)
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 20, mask);
                if (hit.collider != null)
                {
                    hitEvent?.Invoke(hit);
                    HitObject = hit.collider.gameObject.name;
                }
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
}
