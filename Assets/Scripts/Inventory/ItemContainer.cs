using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ContainerType
{
    Pickup,
    Interact,
    Enemy,
}
public class ItemContainer : MonoBehaviour
{

    public Item containedItem;
    
    public ContainerType containerType;

    public float harvestCost;
    public int hitsToHarvest;

    public bool openable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (containerType == ContainerType.Pickup)
            {

                other.gameObject.GetComponent<Inventory>().AddItem(containedItem);
                Destroy(gameObject);
            }
        }
    }
}
