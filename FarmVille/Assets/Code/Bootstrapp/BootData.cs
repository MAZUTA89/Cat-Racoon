using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Bootstrapp
{
    public class BootData : MonoBehaviour
    {
        public static string Str = "sdsd";
        public static BootData instance;
        private void Awake()
        {
            if(instance != null)
            {
                Debug.Log($"Find another {typeof(BootData)} on {gameObject.name}");
                Destroy(this);
                return;
            }
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
        }
        private void Update()
        {
            
        }
    }
}
