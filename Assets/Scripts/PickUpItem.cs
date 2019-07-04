using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldState
{
    notHoldingItem,
    delay,
    holdingItem,
    putItemBack,
    itemBehindWall

}
public class PickUpItem : MonoBehaviour {

    public SmoothMouseLook smoothMouseLook;


    public LayerMask wallLayer;
    private Transform heldObject = null;
    public Item heldItemData;
    public float pickUpRange;
    public float pickUpRadius;
    public Transform itemHeldTarget;
    public Transform inspectItemTarget;
    public float throwForce;
    public float holdDelay;
    public float holdTimer;
    public float maxInteractAngle;
    public float putBackDistance;
    public float distanceToHeldObject;

    public bool canPutBack;
    public bool holdingObject = false;
    public bool objectBehindWall;

    public HoldState currentHoldState;

    // Use this for initialization
    void Start () {
        holdTimer = holdDelay;
      
        
        currentHoldState = HoldState.notHoldingItem;
    }


    public void PickUp()
    {
       
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        distanceToHeldObject = Vector3.Distance(transform.position, heldObject.position);
        heldItemData = heldObject.GetComponent<Item>();
        heldObject.GetComponent<Collider>().isTrigger = true;
        heldObject.position = itemHeldTarget.position;
        heldObject.rotation = itemHeldTarget.rotation;
        heldObject.parent = itemHeldTarget;
        heldObject.gameObject.layer = LayerMask.NameToLayer("NOCLEAR");
        holdingObject = true;
        currentHoldState = HoldState.delay;
;

    }
    public void PutDown()
    {
        heldObject.gameObject.layer = LayerMask.NameToLayer("Item");
        heldObject.parent = null;
        heldObject.GetComponent<Collider>().isTrigger = false;
        heldItemData = null;
        canPutBack = false;
        heldObject = null;
        holdingObject = false;
        currentHoldState = HoldState.notHoldingItem;


    }
    public bool CanPutItemBack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.SphereCast(ray, 1.0f, out hit, putBackDistance);
        if (Mathf.Abs(Vector3.Distance(hit.point, heldItemData.originalLocation)) < putBackDistance)
        {
           
            return true;
        }
        else
        {
          
            return false;

        }

    }

    public bool IsItemBehindWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (heldObject.position - transform.position), out hit, distanceToHeldObject, wallLayer))
        {
            return true;

        }
        else
        {

            return false;
        }
    }
    public GameObject DetectObject(string tagToDetect)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray, pickUpRadius, pickUpRange);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == tagToDetect)
            {
                Vector3 angle = (hit.point- Camera.main.transform.position).normalized;
                RaycastHit testRay;
                if (Physics.Raycast(Camera.main.transform.position, angle.normalized, out testRay, 1000.0f))
                {
                    if (hit.collider.tag == tagToDetect)
                    {
                        float angleTo = Vector3.Angle(angle, Camera.main.transform.forward);
                        if (Mathf.Abs(angleTo) < maxInteractAngle)
                        {
                            return hit.collider.gameObject;
                        }
                    }
                }
            }
            

        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            if (hit.collider.tag == "Interact")
            { 
                if (Input.GetButtonDown("Interact"))
                {
                    hit.collider.gameObject.GetComponent<GameEventTrigger>().TriggerEvent();

                }
                return;
            }

        }



        switch (currentHoldState)
        {
            case HoldState.notHoldingItem:
                GameObject obj = DetectObject("Pickup");
                if (obj)
                {
                    
                    if (Input.GetButtonDown("Interact"))
                    {
                        heldObject = obj.transform;
                        PickUp();
                    }

                }
                else
                {
                }
                break;
            case HoldState.holdingItem:
                if (CanPutItemBack())
                {
                    currentHoldState = HoldState.putItemBack;
                }
                if (Input.GetButtonDown("Interact"))
                {
                    
                    if (IsItemBehindWall())
                    {
                        return;
                    }
                    else
                    {
                        heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        heldObject.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
                        PutDown();

                    }
                }
                break;
            case HoldState.delay:
                if (holdTimer > 0)
                {
                    holdTimer -= Time.deltaTime;

                }
                else
                {
                    holdTimer = holdDelay;
                    currentHoldState = HoldState.holdingItem; 
                }
                break;
            case HoldState.putItemBack:
                if (!CanPutItemBack())
                {
                    currentHoldState = HoldState.holdingItem;
                }
              

                if (Input.GetButton("Interact"))
                {

                    heldObject.transform.position = heldItemData.originalLocation;
                    heldObject.transform.rotation = heldItemData.originalRotation;
                    PutDown();
                }
                break;
        }

    }
}
