using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public int shootDamage;
    public float shootRate;
    public int shootRange;
    public GameObject model;
    public AudioClip gunShotAud;
    [Range(0, 1)] public float gunShotAudVol;
    public int ammo;
}
