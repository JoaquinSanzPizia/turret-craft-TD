using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EffectDataSO : ScriptableObject
{
    [Header("==EXTRA SHOT==")]
    [Space]
    public float[] extraShotChance;

    [Header("==CRIT CHANCE==")]
    [Space]
    public float[] critChance;

    [Header("==RANDOM PIERCE==")]
    [Space]
    public float[] pierceChance;
}
