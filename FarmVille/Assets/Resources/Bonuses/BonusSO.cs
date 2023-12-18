using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MoneyBonus", menuName = "ScriptableObjects/MoneyBonus")]
public class BonusSO : ScriptableObject
{
    [Range(0, 20)]
    public float PeriodTime;
    [Range(0, 100)]
    public int Chance;
}
