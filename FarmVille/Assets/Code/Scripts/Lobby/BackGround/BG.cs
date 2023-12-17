using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    void Start()
    {
        var height = Camera.main.orthographicSize * 2f;
        var width = height * Screen.width / Screen.height;

        transform.localScale = new Vector3(width, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
