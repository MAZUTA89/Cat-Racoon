using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Boot.Communication
{
    public class Progress : MonoBehaviour
    {
        [SerializeField] GameObject LoadImage;

        private void Start()
        {
            CommunicationEvents.OnStartCommunicateEvent += OnStartCommunicate;
        }

        void OnStartCommunicate()
        {
            LoadImage.SetActive(false);
        }
    }
}
