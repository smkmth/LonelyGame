using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpItem : MonoBehaviour {

    public LayerMask interactLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    private Transform heldObject = null;
    public Item heldItemData;
    float dist;
    Rigidbody rb = null;
    //public float pushForce;
    public float pickUpRange;
    public float pickUpRadius;
    public Transform itemHeldTarget;
    public Transform inspectItemTarget;
    public bool holdingObject;
    public float throwForce;
    public float holdDelay;
    public float holdTimer;
    public float speed;
    public SmoothMouseLook smoothMouseLook;
    public float maxAngle;
    public float putBackDistance;
    public bool canPutBack;
    public TextMeshProUGUI itemPrompt;
    public bool objectBehindWall;
    public float distanceToHeldObject;

    // Use this for initialization
    void Start () {
        holdTimer = holdDelay;
        itemPrompt.text = "";
    }


    public void PickUp()
    {
        itemPrompt.text = "";
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        distanceToHeldObject = Vector3.Distance(transform.position, heldObject.position);
        heldItemData = heldObject.GetComponent<Item>();
        heldObject.GetComponent<Collider>().isTrigger = true;
        heldObject.position = itemHeldTarget.position;
        heldObject.rotation = itemHeldTarget.rotation;
        heldObject.parent = itemHeldTarget;
        heldObject.gameObject.layer = LayerMask.NameToLayer("NOCLEAR");
        holdingObject = true;

    }
    public void PutDown()
    {
        heldObject.gameObject.layer = LayerMask.NameToLayer("Draggable");
        heldObject.parent = null;
        heldObject.GetComponent<Collider>().isTrigger = false;
        heldItemData = null;
        canPutBack = false;
        heldObject = null;
        holdingObject = false;
        itemPrompt.text = "";

    }
    // Update is called once per frame
    void Update()
    {

        if (!holdingObject)
        {
            holdTimer = holdDelay;


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, pickUpRadius, pickUpRange);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "Item")
                {
                    Debug.Log(hit.collider.name);
                    Vector3 angle = (hit.transform.position - Camera.main.transform.position).normalized;
                    RaycastHit testRay;
                    if (Physics.Raycast(Camera.main.transform.position, angle.normalized, out testRay, 1000.0f))
                    {
                        if (testRay.collider.tag == "Item")
                        {
                            float angleTo = Vector3.Angle(angle, Camera.main.transform.forward);
                            if (Mathf.Abs(angleTo) < maxAngle)
                            {
                                itemPrompt.text = "Pick up " + hit.collider.gameObject.GetComponent<Item>().itemname;

                                if (Input.GetButtonDown("Interact"))
                                {
                                    heldObject = hit.collider.gameObject.transform;
                                    PickUp();

                                }
                                return;
                            }
                        }
                    }
                }
                else
                {
                    itemPrompt.text = "";

                }
            }
        }

        if (holdingObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            Physics.SphereCast(ray, 1.0f, out hit, putBackDistance);
            if (Mathf.Abs(Vector3.Distance(hit.point, heldItemData.originalLocation)) < putBackDistance)
            {
                canPutBack = true;
                itemPrompt.gameObject.SetActive(true);
                itemPrompt.text = "Put back " + heldItemData.itemname;


            }
            else
            {

                canPutBack = false;
                itemPrompt.gameObject.SetActive(false);

            }
            if (Physics.Raycast(transform.position, (heldObject.position - transform.position), out hit, distanceToHeldObject, wallLayer))
            {
                objectBehindWall = true;

            }
            else
            {

                objectBehindWall = false;
            }
        }
        if (holdTimer <= 0)
        {
            if (Input.GetMouseButtonDown(1) && holdingObject)
            {
                heldObject.transform.position = inspectItemTarget.position;
                heldObject.transform.rotation = inspectItemTarget.rotation;

            }

            if (Input.GetMouseButton(1) && holdingObject)
            {
                smoothMouseLook.cameraControl = false;
                heldObject.transform.Rotate(heldObject.up,Input.GetAxis("Mouse X"),Space.World);

            }
            if (!Input.GetMouseButton(1) && holdingObject)
            {
                smoothMouseLook.cameraControl = true;
                heldObject.position = itemHeldTarget.position;
                heldObject.localRotation = itemHeldTarget.localRotation;

            }

            if (Input.GetButtonDown("Interact") && holdingObject)
            {
                holdTimer = holdDelay;

                if (canPutBack)
                {
                    heldObject.transform.position = heldItemData.originalLocation;
                    heldObject.transform.rotation = heldItemData.originalRotation;
                    PutDown();
                    return;
                }
                else
                {

                    if (objectBehindWall)
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
  
            }
        }
        else
        {
            holdTimer -= Time.deltaTime;
        }
    }
}
