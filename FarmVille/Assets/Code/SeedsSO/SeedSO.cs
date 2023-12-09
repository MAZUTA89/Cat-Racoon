using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Seed", menuName = "ScriptableObjects/Seed")]
public class SeedSO : ScriptableObject
{
    public Item SeedType;
    public float GrowingTime;
    public Sprite GrowingUpSprite;
    public float Money;
}
