using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class DamageEngine
{
    private static float[,] damageAdjustments = new float[,] 
    {
        { 1f, 0.5f, 2f },
        { 2f, 1f, 0.5f },
        { 0.5f, 2f, 1f }
    };

    public static float ComputeDamage(EffectTypes attackType, EffectTypes defenseType, float baseDamage)
    {
        int attackIndex = (int)attackType;
        int defenseIndex = (int)defenseType;
        float modifier = damageAdjustments[attackIndex, defenseIndex];

        return baseDamage * modifier;
    }
}