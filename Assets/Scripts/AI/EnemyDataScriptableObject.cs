using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Sedna Games Custom/Enemy Data")]
public class EnemyDataScriptableObject : ScriptableObject
{
    public float damage;
    public float attackSpeed;
    public float hp;
    public bool isRanged = true;
    public bool dieOnAttack;
    public bool hasExplosiveDamageFalloff;
    public float explosiveDamageFalloff;
    public bool hasKnockback;
    public float knockback;


    //neat things that are NYE
    public float hearingRange;
    public bool searchForLowestHPplayer;
    public float sizeVarianceFloor;
    public float sizeVarianceCieling;
}
