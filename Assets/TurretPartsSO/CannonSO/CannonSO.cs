using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CannonSO : ScriptableObject
{
    public string bulletType;
    public float[] damage;
    public float[] fireRateMultiplier;
    public float[] rangeMultiplier;
    public float[] bulletSpeedMultiplier;
}
