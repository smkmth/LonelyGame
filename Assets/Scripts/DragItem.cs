using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour {

    public LayerMask interactLayer;
    private Transform heldObject = null;
    float dist;
    Rigidbody rb =null;
    public float pushForce;
    public float pushRange;

    void Update()
    {

        if (Input.GetButton("Interact"))
        {
            
            Debug.Log("Hit");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, pushRange, interactLayer))
            {
                rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                rb.AddForceAtPosition(transform.forward *pushForce, hit.point ,ForceMode.Force);

            }


        }


    }

}
