using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{

    Rigidbody rb;
    public float moveSpeed;
    public GameObject playerCamera;
    public float maxSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }




    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 movementVector = new Vector3(inputX, 0, inputY);
        movementVector = playerCamera.transform.TransformDirection(movementVector);
        movementVector.y = 0;
        if (rb.velocity.magnitude < maxSpeed)
        {
           rb.AddForce( movementVector * moveSpeed, ForceMode.Acceleration);

        }


    }


}