using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

    public LayerMask interactLayer;
    private Transform heldObject = null;
    float dist;
    Rigidbody rb = null;
    //public float pushForce;
    public float pickUpRange;
    public Transform itemHeldTarget;
    public bool holdingObject;
    public float throwForce;
    public float holdDelay;
    public float holdTimer;

    // Use this for initialization
    void Start () {
        holdTimer = holdDelay;

    }

    // Update is called once per frame
    void Update()
    {
        if (holdTimer <= 0)
        {

            if (Input.GetButtonDown("Interact") && !holdingObject)
            {
                holdTimer = holdDelay;


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(transform.position,transform.forward, out hit, pickUpRange, interactLayer))
                {
                    Debug.Log("Hit");
                    hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject = hit.collider.gameObject.transform;
                    heldObject.position = itemHeldTarget.position;
                    heldObject.localRotation = itemHeldTarget.localRotation;
                    heldObject.parent = itemHeldTarget;
                    holdingObject = true;
                    return;


                }
            }
            if (Input.GetButtonDown("Interact") && holdingObject)
            {
                holdTimer = holdDelay;

                heldObject.parent = null;
                heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
                heldObject = null;
                holdingObject = false;
                return; 

            }
        }
        else
        {
            holdTimer -= Time.deltaTime;
        }
    }
}
