using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{

    public Dictionary<equipmentSlotType,EquipmentSlot> EquipmentSlots = new Dictionary<equipmentSlotType, EquipmentSlot>();
    public Weapon equipedWeapon;
    public MeshFilter head;
    public MeshFilter legs;
    public MeshFilter torso;
    public MeshFilter arms;
    public MeshFilter weapon;
    public Dictionary<equipmentSlotType,MeshFilter> MeshSlots = new Dictionary<equipmentSlotType, MeshFilter>();

    private void Awake()
    {
        EquipmentSlots.Add(equipmentSlotType.Head, new EquipmentSlot(null, head, equipmentSlotType.Head));
        EquipmentSlots.Add(equipmentSlotType.Legs, new EquipmentSlot(null, legs, equipmentSlotType.Legs));
        EquipmentSlots.Add(equipmentSlotType.Torso, new EquipmentSlot(null, torso, equipmentSlotType.Torso));
        EquipmentSlots.Add(equipmentSlotType.Arms, new EquipmentSlot(null, arms, equipmentSlotType.Arms));

        MeshSlots.Add(equipmentSlotType.Head, head);
        MeshSlots.Add(equipmentSlotType.Legs, legs);
        MeshSlots.Add(equipmentSlotType.Torso,torso);
        MeshSlots.Add(equipmentSlotType.Arms, arms);
        MeshSlots.Add(equipmentSlotType.Weapon, weapon);

    }



    public void EquipItem( Equipment equipment)
    {
        EquipmentSlots[equipment.equipmentSlot].equipment = equipment;
        MeshSlots[equipment.equipmentSlot].mesh = equipment.mesh;

    }

    public void UnEquipItem(Equipment equipment)
    {
        EquipmentSlots[equipment.equipmentSlot].equipment = null;
        MeshSlots[equipment.equipmentSlot].mesh = null;

    }

    public void EquipWeapon(Weapon weapon)
    {
        equipedWeapon = weapon;
        MeshSlots[weapon.equipmentSlot].mesh = weapon.mesh;
    }

    public void UnEquipWeapon()
    {
        equipedWeapon = null;
        MeshSlots[equipmentSlotType.Weapon].mesh = null;

    }
}



public class EquipmentSlot
{

    public Equipment equipment; 
    public MeshFilter whereItGoes;
    public equipmentSlotType slot;

    public EquipmentSlot(Equipment equipment, MeshFilter whereItGoes, equipmentSlotType slot)
    {
        this.equipment = equipment;
        this.whereItGoes = whereItGoes;
        this.slot = slot;
    }
}