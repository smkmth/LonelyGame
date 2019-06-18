using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour
{
    public AudioClip[] footsteps;
    private AudioSource audioSource;
    Rigidbody rb;
    public float moveSpeed;
    public GameObject playerCamera;
    public float maxSpeed;
    public bool walking;
    private float footsteptimer =0.0f;
    public float footstepTime;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
    }


    public void Update()
    {
        int index = Random.Range(0, footsteps.Length);
        AudioClip footstep = footsteps[index];
        if (footsteptimer >= footstepTime)
        {
            if (walking)
            {
                audioSource.clip = footstep;
                audioSource.PlayOneShot(footstep, 0.5f);
                footsteptimer = 0;
            }
        }
        else
        {
            footsteptimer += Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector3 movementVector = new Vector3(inputX, 0, inputY);
        if (movementVector != Vector3.zero)
        {
            walking = true;
        }
        else
        {
            walking = false;

        }
        movementVector = playerCamera.transform.TransformDirection(movementVector);
        movementVector.y = 0;
        if (rb.velocity.magnitude < maxSpeed)
        {
           rb.AddForce( movementVector * moveSpeed, ForceMode.Acceleration);

        }


    }


}