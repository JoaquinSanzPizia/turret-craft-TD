using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpperChasisSO : ScriptableObject
{
    public enum ExtraEffect { bonusShot, critChance, rampUpDamage}
    public ExtraEffect extraEffect;
    public float[] elementBoosts;
}
