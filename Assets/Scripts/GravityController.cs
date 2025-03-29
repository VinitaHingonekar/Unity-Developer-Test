using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private Vector3 gravityDirection = Vector3.down;
    private Vector3 selectedGravity = Vector3.down;

    public float gravityStrength = 9.81f;

    [Header("Player Components")]
    public Rigidbody rb;
    public CharacterMovement playerMovement;

    [Header("Hologram")]
    public GameObject hologramPrefab;
    public List<Transform> hologramPositions;

    private int positionIndex = 0;
    private GameObject hologramInstance;

    private bool isGravityChangeMode = false;

    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on player object!");
            return;
        }

        rb.useGravity = false;  // Disable Unity's default gravity

        // Instantiate hologram but keep it hidden initially
        if (hologramPrefab)
        {
            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
            hologramInstance.SetActive(false);
        }
    }

    void Update()
    {
        isGravityChangeMode = Input.GetKey(KeyCode.LeftShift);

        if (hologramInstance)
            hologramInstance.SetActive(isGravityChangeMode);

        if (isGravityChangeMode)
            HandleGravitySelection();

        if (Input.GetKeyDown(KeyCode.Return) && isGravityChangeMode)
            ApplyGravity();
    }

    void FixedUpdate()
    {
        // Apply custom gravity force
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    }

    void HandleGravitySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            positionIndex = (positionIndex + 1) % hologramPositions.Count; // Cycle forward
            UpdateHologram();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionIndex = (positionIndex - 1 + hologramPositions.Count) % hologramPositions.Count; // Cycle backward
            UpdateHologram();
        }
    }

    void UpdateHologram()
    {
        Transform targetTransform = hologramPositions[positionIndex];

        // Get the "down" direction from the target's transform
        selectedGravity = -targetTransform.up;

        if (hologramInstance)
        {
            hologramInstance.transform.position = targetTransform.position;
            hologramInstance.transform.rotation = targetTransform.rotation;
        }

        Debug.Log("Selected Gravity: " + selectedGravity);
    }

    void ApplyGravity()
    {
        Transform targetTransform = hologramPositions[positionIndex];

        // Move player to hologram position and adjust rotation
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;

        // Apply new gravity direction
        gravityDirection = selectedGravity;
        rb.velocity = Vector3.zero; // Reset velocity to prevent unwanted motion
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);

        // Update gravity in movement script if applicable
        playerMovement.SetGravity(gravityDirection);

        Debug.Log("Applied Gravity: " + gravityDirection);
    }
}
