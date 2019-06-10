using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactable {

    private Crafter crafter;

    public void Start()
    {
        crafter = GetComponent<Crafter>();
    }

    public override void UseInteractable(GameObject user)
    {

        user.GetComponent<PlayerInteraction>().SetCraftingMode(crafter);
    }

        
}
