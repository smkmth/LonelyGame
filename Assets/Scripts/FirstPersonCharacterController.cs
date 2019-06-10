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

    [Tooltip("If checked and the player is on an object tagged \"Slide\", he will slide down it regardless of the slope limit.")]
    [SerializeField]
    private bool SlideOnTaggedObjects = false;

    [Tooltip("How fast the player slides when on slopes as defined above.")]
    [SerializeField]
    private float SlideSpeed = 12.0f;

    [Tooltip("If checked, then the player can change direction while in the air.")]
    [SerializeField]
    private bool AirControl = false;

    [Tooltip("Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast.")]
    [SerializeField]
    private float AntiBumpFactor = .75f;

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
        // If the run button is set to toggle, then switch between walk/run speed. (We use Update for this...
        // FixedUpdate is a poor place to use GetButtonDown, since it doesn't necessarily run every frame and can miss the event)
        if (ToggleRun && Grounded && Input.GetButtonDown("Run"))
        {
            Speed = (Speed == WalkSpeed ? RunSpeed : WalkSpeed);
        }
    }


    private void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && LimitDiagonalSpeed) ? .7071f : 1.0f;
        if (inputX == 0.0f && inputY == 0.0f) {
            AntiBumpFactor = 0.0f;
        }
        else
        {
            AntiBumpFactor = .75f;
        }

        if (Grounded)
        {
            bool sliding = false;
            // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
            // because that interferes with step climbing amongst other annoyances
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

            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (Falling)
            {
                Falling = false;
                if (Transform.position.y < FallStartLevel - FallingThreshold)
                {
                    OnFell(FallStartLevel - Transform.position.y);
                }
            }

            // If running isn't on a toggle, then use the appropriate speed depending on whether the run button is down
            if (!ToggleRun)
            {
                Speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
            }

            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if ((sliding && SlideWhenOverSlopeLimit) || (SlideOnTaggedObjects && Hit.collider.tag == "Slide"))
            {
                Vector3 hitNormal = Hit.normal;
                MoveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref MoveDirection);
                MoveDirection *= SlideSpeed;
                PlayerControl = false;
                Debug.Log("sliding");
            }
            // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
            else
            {
                    MoveDirection = new Vector3(inputX * inputModifyFactor, -AntiBumpFactor, inputY * inputModifyFactor);
                    MoveDirection = Transform.TransformDirection(MoveDirection) * Speed;
                    PlayerControl = true;
                    Debug.Log("mi");
                
            }

            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
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
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!Falling)
            {
                Falling = true;
                FallStartLevel = Transform.position.y;
            }

            // If air control is allowed, check movement but don't touch the y component
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


    // Store point that we're in contact with for use in FixedUpdate if needed
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ContactPoint = hit.point;
    }


    // This is the place to apply things like fall damage. You can give the player hitpoints and remove some
    // of them based on the distance fallen, play sound effects, etc.
    private void OnFell(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }
}