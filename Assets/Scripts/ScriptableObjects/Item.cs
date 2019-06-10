using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon, 
    Resource,
    KeyItem,
    Food,
    Equipment
}

public enum effectType
{
    nothing,
    poison,
    bleeding,
    instaKill,
    slowed,
    staggered
}

public enum equipmentSlotType
{
    Head,
    Legs,
    Torso,
    Arms,
    Weapon
}

[CreateAssetMenu(fileName = "Item", menuName = "Items/ItemData", order = 51)]
public class Item : ScriptableObject 
{
    [Tooltip("The name that will display in the game- make sure this is a unique name , else it wont work")]
    public string title;
    [TextArea]
    [Tooltip("A short description of the object, not used at the moment")]
    public string description;
    [Tooltip("What icon will be displayed for this object in menus")]
    public Sprite icon;
    [Tooltip("How the inventory will treat this object. Make sure it is set right, else it wont work")]
    public ItemType type;

}

