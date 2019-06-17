using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterController : MonoBehaviour {

    [Tooltip("How fast the player moves when walking (default move speed).")]
    [SerializeField]
    private float WalkSpeed = 6.0f;

    [Tooltip("How fast the player moves when running.")]
    [SerializeField]
    private float RunSpeed = 11.0f;

    [Tooltip("If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster.")]
    [SerializeField]
    public bool LimitDiagonalSpeed = true;

    [Tooltip("If checked, the run key toggles between running and walking. Otherwise player runs if the key is held down.")]
    [SerializeField]
    private bool ToggleRun = false;

    [Tooltip("How high the player jumps when hitting the jump button.")]
    [SerializeField]
    private float JumpSpeed = 8.0f;

    [Tooltip("How fast the player falls when not standing on anything.")]
    [SerializeField]
    private float Gravity = 20.0f;

    [Tooltip("Units that player can fall before a falling function is run. To disable, type \"infinity\" in the inspector.")]
    [SerializeField]
    private float FallingThreshold = 10.0f;

    [Tooltip("If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down.")]
    [SerializeField]
    private bool SlideWhenOverSlopeLimit = false;



    [Tooltip("How fast the player slides when on slopes as defined above.")]
    [SerializeField]
    private float SlideSpeed = 12.0f;

    [Tooltip("If checked, then the player can change direction while in the air.")]
    [SerializeField]
    private bool AirControl = false;

    [Tooltip("Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast.")]
    [SerializeField]
    private float AntiBumpFactor;

    [Tooltip("Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping.")]
    [SerializeField]
    private int AntiBunnyHopFactor = 1;

    private Vector3 MoveDirection = Vector3.zero;
    private bool Grounded = false;
    private CharacterController Controller;
    private Transform Transform;
    private float Speed;
    private RaycastHit Hit;
    private float FallStartLevel;
    private bool Falling;
    private float SlideLimit;
    private float RayDistance;
    private Vector3 ContactPoint;
    private bool PlayerControl = false;
    private int JumpTimer;


    private void Start()
    {
        // Saving component references to improve performance.
        Transform = GetComponent<Transform>();
        Controller = GetComponent<CharacterController>();

        // Setting initial values.
        Speed = WalkSpeed;
        RayDistance = Controller.height * .5f + Controller.radius;
        SlideLimit = Controller.slopeLimit - .1f;
        JumpTimer = AntiBunnyHopFactor;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (ToggleRun && Grounded && Input.GetButtonDown("Run"))
        {
            Speed = (Speed == WalkSpeed ? RunSpeed : WalkSpeed);
        }
    }


    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && LimitDiagonalSpeed) ? .7071f : 1.0f;


        if (inputX == 0.0f && inputY == 0.0f) {
            AntiBumpFactor = 0.0f;
        }
        else
        {
            AntiBumpFactor = .4f;
        }

        if (Grounded)
        {
            bool sliding = false;

            if (Physics.Raycast(Transform.position, -Vector3.up, out Hit, RayDistance))
            {
                if (Vector3.Angle(Hit.normal, Vector3.up) > SlideLimit)
                {
                    sliding = true;
                }
            }
            // However, just raycasting straight down from the center can fail when on steep slopes
            // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
            else
            {
                Physics.Raycast(ContactPoint + Vector3.up, -Vector3.up, out Hit);
                if (Vector3.Angle(Hit.normal, Vector3.up) > SlideLimit)
                {
                    sliding = true;
                }
            }

            if (Falling)
            {
                Falling = false;
                if (Transform.position.y < FallStartLevel - FallingThreshold)
                {
                    OnFell(FallStartLevel - Transform.position.y);
                }
            }

            if (!ToggleRun)
            {
                Speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
            }

            if ((sliding && SlideWhenOverSlopeLimit))
            {
                Vector3 hitNormal = Hit.normal;
                MoveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref MoveDirection);
                MoveDirection *= SlideSpeed;
                PlayerControl = false;
                Debug.Log("sliding");
            }
            else
            {
                    
                    MoveDirection = new Vector3(inputX , -AntiBumpFactor, inputY );
                    MoveDirection = Transform.TransformDirection(MoveDirection) * Speed;
                    PlayerControl = true;

                
            }

            if (!Input.GetButton("Jump"))
            {
                JumpTimer++;
            }
            else if (JumpTimer >= AntiBunnyHopFactor)
            {
                MoveDirection.y = JumpSpeed;
                JumpTimer = 0;
            }
        }
        else
        {
            if (!Falling)
            {
                Falling = true;
                FallStartLevel = Transform.position.y;
            }

            if (AirControl && PlayerControl)
            {
                MoveDirection.x = inputX * Speed * inputModifyFactor;
                MoveDirection.z = inputY * Speed * inputModifyFactor;
                MoveDirection = Transform.TransformDirection(MoveDirection);
            }
        }

        // Apply gravity
        MoveDirection.y -= Gravity * Time.deltaTime;

        // Move the controller, and set grounded true or false depending on whether we're standing on something
        Grounded = (Controller.Move(MoveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ContactPoint = hit.point;
    }



    private void OnFell(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }
}