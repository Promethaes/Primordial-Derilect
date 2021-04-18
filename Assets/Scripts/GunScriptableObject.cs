using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Gun Stats",menuName = "Sedna Games Custom/Gun Stats")]
public class GunScriptableObject : ScriptableObject
{
    public float damage;
    public int ammo;
    public float rateOfFire;
    public bool bulletHasArc;
    public float bulletSpeed;
    public bool firesInSpread;
    public float spreadTightness;
    public int bulletsInSpread;
    
    //neat random ideas that are NYE
    public float recoil;
    public float heatCapacity;
    public float accuracy;
    //and so on...
}
