using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// here is the class which handles all inventory stuff. it is player agnoistic
/// so npcs, chests, shops or whatever can use this class
/// </summary>
public class Inventory : MonoBehaviour
{

    public List<ItemSlot> itemSlots;

    //26 item slots
    public int MaxItemSlots;
    public int totalItemsStored;

    // Use this for initialization
    void Start()
    {
        totalItemsStored = 0;

        itemSlots = new List<ItemSlot>();

        for (int i = 0; i <= MaxItemSlots; i++)
        {
            itemSlots.Add(new ItemSlot(null, 0, false));
        }


    }
    public List<int> CheckRequiredItems(List<Item> itemsToCheck)
    {
        List<int> count = new List<int>();
        for(int i = 0; i <= itemsToCheck.Count; i++)
        {
            count[i] = GetItemCount(itemsToCheck[i]);
        }


        return count;

    }

    public int GetItemCount(Item itemToCheck)
    {
        int count =0;
        for (int i = 0; i < MaxItemSlots; ++i)
        {

            if (itemSlots[i].filled)
            {
                if (itemSlots[i].item.title == itemToCheck.title)
                {
                    count = itemSlots[i].quantity;
                    break;
                }

            }
        }

        return count;
    }
    public bool AddItem(Item itemToAdd)
    {
        //check we dont already have this item, if not, then add it, else increase the amount we have of it

        for (int i = 0; i < MaxItemSlots; ++i)
        {

            if (itemSlots[i].filled)
            {
                if (itemSlots[i].item.title == itemToAdd.title)
                {
                    itemSlots[i].quantity += 1;
                    totalItemsStored += 1;
                    return true;
                }

            }
        }
        for (int i = 0; i < MaxItemSlots; ++i)
        {


            if (!itemSlots[i].filled)
            {
                totalItemsStored += 1;
                itemSlots[i].item = itemToAdd;
                itemSlots[i].quantity += 1;
                itemSlots[i].filled = true;
                return true;

            }

        }
        Debug.Log("Inventory full!");
        return false;


    }

    public bool RemoveItem(Item itemToRemove)
    {
        if (itemSlots.Count > 0)
        {
            for (int i = 0; i < MaxItemSlots; ++i)
            {
                if (itemSlots[i].filled ==true)
                {
                    if (itemSlots[i].item.title == itemToRemove.title)
                    {
                        itemSlots[i].quantity -= 1;
                        totalItemsStored -= 1;

                        if (itemSlots[i].quantity <= 0)
                        {
                            itemSlots[i].filled = false;
                            itemSlots[i].item = null;

                        }
                        return true;
                    }
                }
            }
            
        }
        return false;


    }

}
//this class handles the item slot - wish it could be a struct, but quantity and filled have to be mutable, so :(
[System.Serializable]
public class ItemSlot
{
    public Item item;
    public int quantity;
    public bool filled;
    public bool equiped =false;

    public ItemSlot(Item item, int quantity, bool filled)
    {
        this.item = item;
        this.quantity = quantity;
        this.filled = filled;
    }
}