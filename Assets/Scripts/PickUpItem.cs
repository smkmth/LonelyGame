using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

    public LayerMask interactLayer;
    public LayerMask playerLayer;
    private Transform heldObject = null;
    private Vector3 heldObjectPos;
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
                RaycastHit[] hits = Physics.SphereCastAll(ray, pickUpRadius, pickUpRange);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.tag == "Item")
                    {
                        Vector3 angle = (hit.transform.position - Camera.main.transform.position).normalized;
                        Debug.DrawRay(Camera.main.transform.position, angle * 1000000.0f, Color.red ,1000000000.0f);
                        RaycastHit testRay;
                        if (Physics.Raycast(Camera.main.transform.position, angle.normalized, out testRay, 1000.0f))
                        {
                            if (testRay.collider.tag == "Item")
                            { 
                                float angleTo = Vector3.Angle(angle, Camera.main.transform.forward);
                                if(Mathf.Abs(angleTo) < maxAngle)
                                {
                                    heldObjectPos = hit.collider.transform.position;
                                    hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                    heldObject = hit.collider.gameObject.transform;
                                    heldObject.GetComponent<Collider>().isTrigger = true;
                                    heldObject.position = itemHeldTarget.position;
                                    heldObject.rotation = itemHeldTarget.rotation;
                                    heldObject.parent = itemHeldTarget;
                                    holdingObject = true;
                                    return;
                                }

                            }

                        }
                  

                    }
                    


                }
            }

            if (Input.GetMouseButtonDown(1) && holdingObject)
            {
                heldObject.transform.position = inspectItemTarget.position;
                heldObject.transform.rotation = inspectItemTarget.rotation;

            }

            if (Input.GetMouseButton(1) && holdingObject)
            {

                smoothMouseLook.cameraControl = false;

                    heldObject.transform.Rotate(heldObject.right,Input.GetAxis("Mouse Y"),Space.World);

           
                    heldObject.transform.Rotate(heldObject.up,Input.GetAxis("Mouse X"),Space.World);
                
                /*
              //  heldObject.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed , Space.Self);
                if (Input.GetAxisRaw("Mouse Y") == 0 && Input.GetAxisRaw("Mouse X") == 0)
                {
                    Vector3 rotVector = heldObject.transform.eulerAngles;
                    rotVector.x = Mathf.Round(rotVector.x / 45) * 45;
                    rotVector.y = Mathf.Round(rotVector.y / 45) * 45;
                    rotVector.z = Mathf.Round(rotVector.z / 45) * 45;
       
                    heldObject.eulerAngles = rotVector;

                }
                */
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

                heldObject.parent = null;
                heldObject.GetComponent<Collider>().isTrigger = false;
                heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
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
