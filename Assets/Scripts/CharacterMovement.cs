//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharacterMovement : MonoBehaviour
//{
//    public Rigidbody rb;
//    public Animator animator;
//    public Transform cam;

//    public float moveSpeed = 5f;
//    public float turnSpeed = 10f;
//    public LayerMask groundLayer;

//    private bool isGrounded;
//    private Vector3 gravityDirection = Vector3.down;

//    public float jumpForce = 7f;
//    public float jumpCooldown;

//    bool readyToJump = true;

//    void Start()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;

//        rb.useGravity = false;
//        //rb.drag = 0;
//    }

//    void Update()
//    {
//        ManageInput();
//        CheckGround();
//    }

//    void FixedUpdate()
//    {
//        //rb.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);
//        rb.AddForce(gravityDirection * 9.81f, ForceMode.VelocityChange);
//        //rb.velocity += gravityDirection * 9.81f * Time.fixedDeltaTime;
//    }

//    private void ManageInput()
//    {
//        // Dont move the player if selecting gravity
//        if (Input.GetKey(KeyCode.LeftShift))
//            return;

//        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
//            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
//        {
//            return; // Prevent movement when arrow keys are used
//        }

//        float horizontalInput = Input.GetAxisRaw("Horizontal");
//        float verticalInput = Input.GetAxisRaw("Vertical");


//        if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0)
//        {
//            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
//                verticalInput = 0; // Prioritize horizontal movement
//            else
//                horizontalInput = 0; // Prioritize vertical movement
//        }


//        Vector3 rightDirection = Vector3.Cross(-gravityDirection, cam.forward).normalized; // Right based on gravity
//        Vector3 forwardDirection = Vector3.Cross(rightDirection, -gravityDirection).normalized; // Forward based on gravity

//        //Vector3 direction = (horizontalInput * rightDirection + verticalInput * forwardDirection).normalized;

//        Vector3 direction = (horizontalInput * rightDirection + verticalInput * forwardDirection).normalized;


//        //Vector3 direction = new Vector3(-horizontalInput, 0f, -verticalInput).normalized;

//        if (direction.magnitude >= 0.1f)
//        {
//            animator.SetBool("isRunning", true);

//            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

//            // Checking if theres a wall in front
//            Vector3 moveDirection = targetRotation * Vector3.forward;
//            if (!Physics.Raycast(transform.position, moveDirection, 0.6f, groundLayer))
//            {
//                //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
//                rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
//            }
//            else
//            {
//                rb.velocity = new Vector3(0, rb.velocity.y, 0); // Stop movement if blocked
//            }
//        }
//        else
//        {
//            animator.SetBool("isRunning", false);
//            rb.velocity = new Vector3(0, rb.velocity.y, 0);
//        }

//        if (Input.GetButtonDown("Jump") && readyToJump && isGrounded)
//        {
//            readyToJump = false;
//            Jump();
//            Invoke(nameof(ResetJump), jumpCooldown);
//        }
//    }



//    //private void CheckGround()
//    //{
//    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
//    //}

//    void CheckGround()
//    {
//        isGrounded = Physics.Raycast(transform.position, gravityDirection, 1.1f, groundLayer);
//    }

//    //private void Jump()
//    //{
//    //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

//    //    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);


//    //}

//    private void Jump()
//    {
//        Debug.Log("Jump function called!");
//        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset Y velocity
//        rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse); // Jump against gravity
//    }

//    //void Jump()
//    //{
//    //    rb.velocity = Vector3.zero; // Reset velocity
//    //    rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse); // Jump against gravity
//    //}

//    private void ResetJump()
//    {
//        readyToJump = true;
//    }

//    //public void SetGravity(Vector3 newGravity)
//    //{
//    //    gravityDirection = newGravity.normalized;
//    //}

//    //public void SetGravity(Vector3 newGravity)
//    //{
//    //    gravityDirection = newGravity.normalized;
//    //    rb.velocity = Vector3.zero; // Reset velocity when gravity changes
//    //}
//    public void SetGravity(Vector3 newGravity)
//    {
//        gravityDirection = newGravity.normalized;
//        rb.velocity = Vector3.Project(rb.velocity, gravityDirection); // Keep only the gravity velocity
//    }


//}
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravityStrength = 9.81f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private Vector3 gravityDirection = Vector3.down;
    private bool isGrounded;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        rb.useGravity = false; // We manually handle gravity
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {
        HandleMovement();
        CheckGrounded();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void HandleMovement()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        if (shiftHeld) return; // Prevent movement when selecting gravity

        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Dynamically calculate movement directions relative to gravity
        Vector3 rightDirection = Vector3.Cross(-gravityDirection, Camera.main.transform.forward).normalized;
        Vector3 forwardDirection = Vector3.Cross(rightDirection, -gravityDirection).normalized;

        // Movement based on new gravity alignment
        Vector3 moveDirection = (moveX * rightDirection + moveZ * forwardDirection).normalized;

        // Apply movement with velocity
        Vector3 velocity = moveDirection * moveSpeed;
        velocity += Vector3.Project(rb.velocity, gravityDirection); // Preserve gravity effect
        rb.velocity = velocity;

        animator.SetFloat("Speed", moveDirection.magnitude);
        animator.SetBool("isFalling", !isGrounded);
    }

    void Jump()
    {
        rb.velocity += -gravityDirection * jumpForce; // Jump against gravity direction
    }

    void ApplyGravity()
    {
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    }

    void CheckGrounded()
    {
        float checkDistance = 0.1f + capsuleCollider.radius;
        isGrounded = Physics.Raycast(transform.position, gravityDirection, checkDistance);
    }

    public void SetGravity(Vector3 newGravity)
    {
        gravityDirection = newGravity.normalized;
        rb.velocity = Vector3.zero; // Reset velocity when gravity changes

        // Rotate player to align with new gravity
        transform.rotation = Quaternion.FromToRotation(Vector3.up, -gravityDirection);
    }
}
