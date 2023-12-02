using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Code.Scripts.Boot;

public class SetTimer : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = User.LoadTime.ToString();
    }
}
