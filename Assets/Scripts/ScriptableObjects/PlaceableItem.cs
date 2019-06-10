using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum interactableType
{
    Nothing,
    Fire,
    Anvil
}

[CreateAssetMenu(fileName = "Food", menuName = "Items/PlaceableItem", order = 52)]
public class PlaceableItem : Item
{
    public GameObject prefabItemPlaced;
    public interactableType placeableItemType;
}
