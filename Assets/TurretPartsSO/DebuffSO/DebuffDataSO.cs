using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DebuffDataSO : ScriptableObject
{
    [Header("==FIRE==")]
    [Space]
    public float[] burnDamage;
    public float[] burnTicks;
    public float[] burnTickRate;

    [Header("==STEEL==")]
    [Space]
    public float[] bonusDamage;

    [Header("==POISON==")]
    [Space]
    public float[] poisonBonusDamage;
    public float[] poisonDamageRepeat;

    [Header("==ICE==")]
    [Space]
    public float[] slowPower;
    public float[] slowTime;

    [Header("==LIGHNING==")]
    [Space]
    public float[] lightningDamage;
    public float[] lightningCooldown;
}
