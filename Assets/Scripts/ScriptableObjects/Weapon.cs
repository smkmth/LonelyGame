using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "Items/WeaponData", order = 52)]
public class Weapon : Equipment
{
    [Tooltip("How much damage this weapon does")]
    public int attackDamage;
    [Tooltip("Does this item have a special effect?")]
    public SpecialEffect[] effect;
}



