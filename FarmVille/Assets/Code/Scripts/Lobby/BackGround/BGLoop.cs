using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.Lobby.BackGround
{
    public class BGLoop : MonoBehaviour
    {
        public float Speed = 0.1f;
        Vector2 offset;
        Material material;


        private void Start()
        {
             material = GetComponent<Renderer>().material;
            offset = material.GetTextureOffset("_MainTex");
        }
        private void Update()
        {
            offset.x = Mathf.Lerp(offset.x, offset.x +  Speed, Time.deltaTime);
            material.SetTextureOffset("_MainTex", offset);
        }
    }
}
