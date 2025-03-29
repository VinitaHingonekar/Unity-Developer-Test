//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GravityController : MonoBehaviour
//{
//    Vector3 selectedGravity = Vector3.down;
//    bool isGravityChangeMode = false;

//    public float gravityStrength = 9.81f;
//    private Vector3 gravityDirection = Vector3.down;

//    public GameObject hologramPrefab;
//    private GameObject hologramInstance;

//    public Rigidbody rb;

//    public List<Transform> hologramPositions;

//    private int positionIndex = 0;
//    private int selectedPosition;

//    public CharacterMovement playerMovement;
//    void Start()
//    {

//        rb.useGravity = false;

//        if (hologramPrefab)
//        {
//            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
//            hologramInstance.SetActive(false);
//        }

//        //Physics.gravity = selectedGravity * gravityStrength;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        isGravityChangeMode = Input.GetKey(KeyCode.LeftShift);

//        if (hologramInstance)
//            hologramInstance.SetActive(isGravityChangeMode);

//        if (isGravityChangeMode)
//            HandleGravitySelection();

//        if (Input.GetKeyDown(KeyCode.Return) && isGravityChangeMode)
//            ApplyGravity();
//    }

//    void FixedUpdate()
//    {
//        // Apply constant gravity
//        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
//    }

//    void HandleGravitySelection()
//    {
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            positionIndex = (positionIndex + 1) % hologramPositions.Count; // Cycle forward
//            SelectGravity(hologramPositions[positionIndex].transform);
//        }
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            positionIndex = (positionIndex - 1 + hologramPositions.Count) % hologramPositions.Count; // Cycle backward
//            SelectGravity(hologramPositions[positionIndex].transform);
//        }

//    }

//    void SelectGravity(Transform targetTransform)
//    {
//        selectedGravity = (targetTransform.position - transform.position).normalized;
//        selectedPosition = positionIndex;

//        if (hologramInstance)
//        {
//            hologramInstance.transform.position = targetTransform.position;
//            hologramInstance.transform.rotation = targetTransform.rotation;
//        }

//        Debug.Log("Selected Gravity: " + selectedGravity); // Debugging

//    }

//    void ApplyGravity()
//    {
//        // Move the player to the hologram's position
//        //transform.position = hologramInstance.transform.position;

//        transform.position = hologramPositions[positionIndex].transform.position;
//        transform.rotation = hologramPositions[positionIndex].transform.rotation;


//        // Set the new gravity direction
//        gravityDirection = selectedGravity;

//        // Apply gravity to the Rigidbody manually
//        rb.velocity = Vector3.zero; // Reset velocity to prevent unwanted movement
//        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);

//        // Apply gravity to the player movement script (if applicable)
//        playerMovement.SetGravity(gravityDirection);

//        Debug.Log("Applied Gravity: " + gravityDirection);
//    }

//    //void ApplyGravity()
//    //{
//    //    // Move the player to the hologram position
//    //    transform.position = hologramPositions[positionIndex].position;

//    //    // Rotate player to align with new gravity
//    //    //Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -selectedGravity) * transform.rotation;
//    //    //transform.rotation = targetRotation;

//    //    // Set new gravity direction
//    //    gravityDirection = selectedGravity;

//    //    // Reset velocity before applying gravity
//    //    rb.velocity = Vector3.zero;

//    //    Debug.Log("Applied Gravity: " + gravityDirection);
//    //}

//}










//using System.Collections.Generic;
//using UnityEngine;

//public class GravityController : MonoBehaviour
//{
//    public float gravityStrength = 9.81f;
//    private Vector3 gravityDirection = Vector3.down;
//    private Vector3 selectedGravity = Vector3.down;

//    public GameObject hologramPrefab;
//    private GameObject hologramInstance;

//    public Rigidbody rb;
//    public CharacterMovement playerMovement;

//    public List<Transform> hologramPositions;
//    private int positionIndex = 0;

//    private bool isGravityChangeMode = false;

//    void Start()
//    {
//        if (rb == null)
//        {
//            Debug.LogError("Rigidbody missing on player object!");
//            return;
//        }

//        rb.useGravity = false;  // Disable Unity's default gravity

//        // Instantiate hologram but keep it hidden initially
//        if (hologramPrefab)
//        {
//            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
//            hologramInstance.SetActive(false);
//        }
//    }

//    void Update()
//    {
//        isGravityChangeMode = Input.GetKey(KeyCode.LeftShift);

//        if (hologramInstance)
//            hologramInstance.SetActive(isGravityChangeMode);

//        if (isGravityChangeMode)
//            HandleGravitySelection();

//        if (Input.GetKeyDown(KeyCode.Return) && isGravityChangeMode)
//            ApplyGravity();
//    }

//    void FixedUpdate()
//    {
//        // Apply custom gravity force
//        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
//    }

//    void HandleGravitySelection()
//    {
//        if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            positionIndex = (positionIndex + 1) % hologramPositions.Count; // Cycle forward
//            UpdateHologram();
//        }
//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            positionIndex = (positionIndex - 1 + hologramPositions.Count) % hologramPositions.Count; // Cycle backward
//            UpdateHologram();
//        }
//    }

//    void UpdateHologram()
//    {
//        Transform targetTransform = hologramPositions[positionIndex];
//        //selectedGravity = (targetTransform.position - transform.position).normalized;
//        selectedGravity = -targetTransform.up;

//        if (hologramInstance)
//        {
//            hologramInstance.transform.position = targetTransform.position;
//            hologramInstance.transform.rotation = targetTransform.rotation;
//        }

//        Debug.Log("Selected Gravity: " + selectedGravity);
//    }

//    void ApplyGravity()
//    {
//        Transform targetTransform = hologramPositions[positionIndex];

//        // Move player to hologram position and adjust rotation
//        transform.position = targetTransform.position;
//        transform.rotation = targetTransform.rotation;

//        // Apply new gravity direction
//        gravityDirection = selectedGravity;
//        rb.velocity = Vector3.zero; // Reset velocity to prevent unwanted motion
//        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);

//        // Update gravity in movement script if applicable
//        playerMovement.SetGravity(gravityDirection);

//        Debug.Log("Applied Gravity: " + gravityDirection);
//    }
//}







using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    private Vector3 gravityDirection = Vector3.down;
    private Vector3 selectedGravity = Vector3.down;

    public GameObject hologramPrefab;
    private GameObject hologramInstance;

    public Rigidbody rb;
    public CharacterMovement playerMovement;

    public List<Transform> hologramPositions;
    private int positionIndex = 0;

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
