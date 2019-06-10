using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum processState
{
    nothingToProcess,
    currentlyProcessing,
    finishedProcessing
}
public class Crafter : MonoBehaviour {

    public interactableType craftingType;
    public Inventory outputInventory;
    public Inventory playerInventory;
    public List<CraftingRecipe> masterCraftingRecipes;
    public Item currentItemProcessing;
    public MasterCraftList craftlist;
    public bool isCurrentlyProcessing;
    public processState currentProcessState;
    public float processTimer;
    private float processTarget;
    private CraftingMenu craftingMenu;

    public void Start()
    {
        craftingMenu = GameObject.Find("Player").GetComponent<CraftingMenu>();
        masterCraftingRecipes = GameObject.Find("SceneManager").GetComponent<MasterCraftList>().MasterCraftRecipies;
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (outputInventory == null)
        {
            outputInventory = GetComponent<Inventory>();
        }
    }

    public void Update()
    {
        if (currentProcessState == processState.currentlyProcessing)
        {
            if (processTimer > 0)
            {

                processTimer -= Time.deltaTime;
                craftingMenu.UpdateProcessing();
            }
            else
            {
                currentProcessState = processState.finishedProcessing;
                craftingMenu.UpdateProcessing();
                processTimer = 0;
            }
        }
    }
    public void FinishProcessing()
    {
        playerInventory.AddItem(currentItemProcessing);
        currentItemProcessing = null;
        currentProcessState = processState.nothingToProcess;
        craftingMenu.UpdateProcessing();


    }

    public List<CraftingRecipe> GetCraftingRecipes()
    {
        List<CraftingRecipe> recipies = new List<CraftingRecipe>();

        for (int i = 0; i < masterCraftingRecipes.Count; i++)
        {
            CraftingRecipe currentCraftingRecipe = masterCraftingRecipes[i];

            if (masterCraftingRecipes[i].requiredType == craftingType)
            {
                recipies.Add(currentCraftingRecipe); 
            }

        }
        return recipies;
    }

    public List<Item> CheckItemsOwned(CraftingRecipe recipeToCheck)
    {
        List<Item> itemsHave = new List<Item>();
        for (int i = 0; i < recipeToCheck.requiredIngredients.Count; i++)
        {
            if (playerInventory.GetItemCount(recipeToCheck.requiredIngredients[i]) >= recipeToCheck.indexedIngredientQuantity[i])
            {
                itemsHave.Add(recipeToCheck.requiredIngredients[i]);

            }
        }
        return itemsHave;
    }

    public List<Item> CheckItemsMissing(CraftingRecipe recipeToCheck)
    {
        List<Item> itemsMissing = new List<Item>();
        for (int i = 0; i < recipeToCheck.requiredIngredients.Count; i++)
        {
            if (playerInventory.GetItemCount(recipeToCheck.requiredIngredients[i]) < recipeToCheck.indexedIngredientQuantity[i])
            {
                itemsMissing.Add(recipeToCheck.requiredIngredients[i]);

            }
        }
        return itemsMissing;
    }

    public int RecipeToIndex(CraftingRecipe recipeToFind)
    {
        int index = -1;
        for (int i = 0; i < masterCraftingRecipes.Count; i++)
        {
            if (masterCraftingRecipes[i].name == recipeToFind.name)
            {
                index = i;
            }
        }
        return index;
    }

    public void CraftItem(CraftingRecipe itemToCraft)
    {
        if (CheckItemsMissing(itemToCraft).Count == 0)
        {
            if (itemToCraft.processTime > 0)
            {
                currentItemProcessing = itemToCraft.itemProduced;
                currentProcessState = processState.currentlyProcessing;
                processTimer = itemToCraft.processTime;
                craftingMenu.UpdateProcessing();
            }
            else
            {
                outputInventory.AddItem(itemToCraft.itemProduced);

            }
            foreach (Item item in itemToCraft.requiredIngredients)
            {
                playerInventory.RemoveItem(item);
            }
        }
    }

}
