using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    public Transform cam;

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private Vector3 gravityDirection = Vector3.down;

    public float jumpForce = 7f;
    public float jumpCooldown;

    bool readyToJump = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ManageInput();
        CheckGround();
    }

    private void ManageInput()
    {
        // Dont move the player if selecting gravity
        if (Input.GetKey(KeyCode.LeftShift))
            return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0)
        {
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                verticalInput = 0; // Prioritize horizontal movement
            else
                horizontalInput = 0; // Prioritize vertical movement
        }

        Vector3 direction = new Vector3(-horizontalInput, 0f, -verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isRunning", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Checking if theres a wall in front
            Vector3 moveDirection = targetRotation * Vector3.forward;
            if (!Physics.Raycast(transform.position, moveDirection, 0.6f, groundLayer))
            {
                //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0); // Stop movement if blocked
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (Input.GetButtonDown("Jump") && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }



    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void SetGravity(Vector3 newGravity)
    {
        gravityDirection = newGravity.normalized;
    }
}
