using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/EquipmentData", order = 52)]
public class Equipment : Item
{
    [Tooltip("What this item looks like when worn")]
    public Mesh mesh;
    [Tooltip("What equipment slot this occupies")]
    public equipmentSlotType equipmentSlot;
}
