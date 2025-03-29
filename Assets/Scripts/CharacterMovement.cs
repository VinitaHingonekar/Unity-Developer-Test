using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
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

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
