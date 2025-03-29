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

        rb.useGravity = false; // disablin unity's gravity

        // instantiating hologram but keep it setting it inactive at the start
        if (hologramPrefab)
        {
            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
            hologramInstance.SetActive(false);
        }
    }

    void Update()
    {
        //only take arrow keys inpput when shift is pressed
        isGravityChangeMode = Input.GetKey(KeyCode.LeftShift);

        if (hologramInstance)
        {
            if (isGravityChangeMode)
            {
                hologramInstance.SetActive(true);
                UpdateHologram(); // so that hologram updates immediately when shift is pressed
            }
            else
            {
                hologramInstance.SetActive(false);
            }
        }

        if (isGravityChangeMode)
            HandleGravitySelection();

        if (Input.GetKeyDown(KeyCode.Return) && isGravityChangeMode)
            ApplyGravity();
    }

    void FixedUpdate()
    {
        // applying custom force to the player
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    }

    void HandleGravitySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            positionIndex = (positionIndex + 1) % hologramPositions.Count; // cycle through the directions (forward)
            UpdateHologram();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionIndex = (positionIndex - 1 + hologramPositions.Count) % hologramPositions.Count; // cycle through the directions (backward)
            UpdateHologram();
        }
    }

    void UpdateHologram()
    {
        Transform targetTransform = hologramPositions[positionIndex];

        // getting the down direction of the hologram
        selectedGravity = -targetTransform.up;

        if (hologramInstance)
        {
            hologramInstance.transform.position = targetTransform.position;
            hologramInstance.transform.rotation = targetTransform.rotation;
        }
        //Debug.Log("Selected Gravity: " + selectedGravity);
    }

    void ApplyGravity()
    {
        Transform targetTransform = hologramPositions[positionIndex];

        // moving player to holograms position and rotation
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;

        // applying new gravity direction
        gravityDirection = selectedGravity;

        rb.velocity = Vector3.zero; 
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);

        // updateing gravity direction in movement script
        playerMovement.SetGravity(gravityDirection);

        //Debug.Log("Applied Gravity: " + gravityDirection);
    }
}
