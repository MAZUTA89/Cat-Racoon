using System;
using UnityEngine;

namespace Assets.Code.Scripts.Boot
{
    public class BootSingleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }
        public void Awake()
        {
            if (Instance != null)
            {
                Debug.Log($"Find another instance on {gameObject.name}");
                Destroy(gameObject);
                return;
            }
            Instance = (T)Convert.ChangeType(this, typeof(T));
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        protected virtual void OnAwake()
        {

        }
    }
}
