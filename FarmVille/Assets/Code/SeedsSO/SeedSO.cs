using Assets.Code;
using UnityEngine;
[CreateAssetMenu(fileName = "Seed", menuName = "ScriptableObjects/Seed")]
public class SeedSO : ScriptableObject
{
    public Item SeedType;
    public float GrowingTime;
    public Sprite GrowingUpSprite;
    public float Money;
}
