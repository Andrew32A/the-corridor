using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed; // default: 7f
    public float sprintSpeed; // default: 14f
    public float groundDrag;
    public float wallrunSpeed;
    public bool isSprinting = false;

    [Header("Jumping")]
    public float jumpForce; // default: 15f
    public float jumpCooldown; // default: 0.25f
    public float airMultiplier; // default: 0.4f
    private bool readyToJump;
    public bool canDoubleJump;
    public bool hasDoubleJumped = false;

    [Header("Crouching")]
    public float crouchSpeed; // default: 3.5f
    public float crouchYScale; // default: 0.5f
    private float startYScale;

    [Header("Input Options")]
    public KeyCode jumpKey = KeyCode.Space; // default: KeyCode.Space
    public KeyCode sprintKey = KeyCode.LeftShift; // default: KeyCode.LeftShift
    public KeyCode crouchKey = KeyCode.LeftControl; // default: KeyCode.LeftControl
    public bool holdToSprint;

    [Header("Ground Check")]
    public float playerHeight; // default: 2f
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool isCrouching;

    [Header("Slope Handling")]
    public float maxSlopeAngle; // default: 0f
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    public Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    void Update()
    {
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // reset double jump when player lands or wallruns
        if (isGrounded || wallrunning)
        {
            hasDoubleJumped = false;
        }

        // handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // get WASD input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // toggle sprinting when moving forward
        if (!holdToSprint && verticalInput > 0 && Input.GetKeyDown(sprintKey))
        {
            isSprinting = !isSprinting;
        }

        // stop sprinting when player stops moving forward or when player speed is less than walk speed
        if (verticalInput <= 0 || rb.velocity.magnitude <= (walkSpeed - 2f))
        {
            isSprinting = false;
        }

        // jump
        if (!canDoubleJump)
        {
            if (Input.GetKeyDown(jumpKey) && readyToJump && isGrounded)
            {
                readyToJump = false;
                Jump();
            }

            // reset jump
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (canDoubleJump)
        {

            if (Input.GetKeyDown(jumpKey) && readyToJump && (isGrounded || (!hasDoubleJumped && !isGrounded)) && !wallrunning)
            {
                if (!isGrounded)
                {
                    hasDoubleJumped = true; // player used their double jump
                }
                readyToJump = false;
                Jump();

                // reset jump
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        // crouch
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // mode - crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // mode - sprinting w/ hold to sprint
        if (isGrounded && Input.GetKey(sprintKey) && !isCrouching && holdToSprint)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // mode - toggled sprinting w/o hold to sprint
        else if (isGrounded && isSprinting && !isCrouching)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // mode - walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }

        // mode - wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // slope movement
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > startYScale)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // grounded movement
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // air movement
        if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // turn off gravity while on a slope
        if (!wallrunning)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        // limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        // limit speed on ground/air
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // add jump force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
