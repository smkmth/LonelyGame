using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Items/FoodData", order = 52)]
public class Food : Item
{
    [Tooltip("How much health this item will restore")]
    public int healthRestore;
    [Tooltip("How much stamina this item will restore")]
    public float staminaRestore;

    public List<SpecialEffect> specialEffects;
}

