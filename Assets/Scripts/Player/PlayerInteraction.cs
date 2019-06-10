using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum interactionState
{
    Normal,
    DialogueMode,
    InventoryMode,
    CraftingMode
}
public class PlayerInteraction : MonoBehaviour
{
    public TimeManager time;



    public bool FreeMove;


    [Header("Rotation Settings")]
    public bool FreeLook;
    public float turnSpeed;

    [Header("Interaction Settings")]
    public float interactRange;
    private List<DialogueLine> receivedDialogue;
    public interactionState currentInteractionState;
    private int dialogueIndex;


    private InkDisplayer dialogueDisplayer;
    private Combat combat;
    public PlayerHUD hud;
    private Inventory inventory;
    private InventoryDisplayer inventoryDisplayer;
    private AnimationManager animator;
    private Crafter playerCrafter;
    private CraftingMenu craftingMenu;

    private float noDialogueTimer = 0.0f;
    public float dialogueTimeOut;
    public bool justLeftDialogue;
    public bool canSpeak = true;




    private void Start()
    {
        playerCrafter = GetComponent<Crafter>();
        inventoryDisplayer = GetComponent<InventoryDisplayer>();
        animator = GetComponent<AnimationManager>();
        inventory = GetComponent<Inventory>();
        craftingMenu = GetComponent<CraftingMenu>();
        dialogueDisplayer = GameObject.Find("SceneManager").GetComponent<InkDisplayer>();

        hud = GetComponent<PlayerHUD>();
        combat = GetComponent<Combat>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FreeMove = true;
        FreeLook= true;

    }

    void SetCursorState(CursorLockMode wantedMode)
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }


    public void SetDialogueMode()
    {
        if (currentInteractionState == interactionState.Normal)
        {
            hud.ToggleHUD(true);
            canSpeak = false;
            Cursor.visible = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.DialogueMode;
        }
        else if (currentInteractionState == interactionState.DialogueMode)
        {
            hud.ToggleHUD(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentInteractionState = interactionState.Normal;
            noDialogueTimer = 0.0f;

        }
    }

    void SetInventoryMode(bool toggleOn)
    {
        if (toggleOn)
        {
            hud.ToggleHUD(true);

            inventoryDisplayer.ToggleInventoryMenu(true);
            Cursor.visible = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.InventoryMode;

        }
        else if (!toggleOn)
        {
            hud.ToggleHUD(false);
            inventoryDisplayer.ToggleInventoryMenu(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentInteractionState = interactionState.Normal;


        }
    }


    public void SetCraftingMode(Crafter crafter)
    {
        if (currentInteractionState == interactionState.Normal)
        {
            craftingMenu.ToggleCraftingMenu(true,crafter);
            hud.ToggleHUD(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            currentInteractionState = interactionState.CraftingMode;

        }
        else if (currentInteractionState == interactionState.CraftingMode)
        {
            craftingMenu.ToggleCraftingMenu(false, crafter);
            hud.ToggleHUD(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            currentInteractionState = interactionState.Normal;


        }
    }



    // Update is called once per frame
    void Update()
    {
   

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorState(CursorLockMode.None);
        }

        if (Input.GetButtonDown("Crafting"))
        {
            SetCraftingMode(playerCrafter);
        }

        switch (currentInteractionState) 
        {

            case interactionState.Normal :

             
    

                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(true);
                }



           
            
                if (Input.GetButtonDown("Interact"))
                {
                    RaycastHit interact;
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100.0f, Color.yellow);
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out interact, interactRange))
                    {


                        if (interact.transform.gameObject.tag == "Item")
                        {
                            ItemContainer item = interact.transform.gameObject.GetComponent<ItemContainer>();
                            if (item.hitsToHarvest == 0)
                            {
                                inventory.AddItem(item.containedItem);
                                Destroy(interact.transform.gameObject);
                            }
                            else
                            {
                                item.hitsToHarvest -= 1;
                            }
                        }
                        if (interact.transform.gameObject.tag == "NPC")
                        {
                            if (canSpeak)
                            {

                                noDialogueTimer = 0.0f;
                                interact.transform.gameObject.GetComponent<DialogueContainer>().Talk();
                                SetDialogueMode();
                                return;
                            }

                        }
                        if (interact.transform.gameObject.tag == "Interactable")
                        {
                            Interactable interactable = interact.transform.gameObject.GetComponent<Interactable>();
                            Debug.Log("hit interact");
                            interactable.UseInteractable(gameObject);
                        }
                    }
                }
                if (!canSpeak)
                {
                    if (noDialogueTimer == dialogueTimeOut)
                    {
                        dialogueTimeOut =+ Time.deltaTime;
                    }
                    else
                    {
                        noDialogueTimer = 0;
                        canSpeak = true;
                    }

                }

                break;
            case interactionState.DialogueMode:
             
                break;
            case interactionState.InventoryMode:
                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(false);
                }

                break;
            case interactionState.CraftingMode:
                if (Input.GetButtonDown("Inventory"))
                {
                    SetInventoryMode(false);
                }

                break;
        }
    }
}
