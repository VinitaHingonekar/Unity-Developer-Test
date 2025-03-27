using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    Vector3 selectedGravity = Vector3.down;
    bool isGravityChangeMode = false;

    public float gravityStrength = 9.81f;
    private Vector3 gravityDirection = Vector3.down;

    public GameObject hologramPrefab;
    private GameObject hologramInstance;

    public Rigidbody rb;

    public List<Transform> hologramPositions;

    private int positionIndex = 0;

    //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        //rb.useGravity = false;

        if (hologramPrefab)
        {
            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
            hologramInstance.SetActive(false);
        }

        Physics.gravity = selectedGravity * gravityStrength;
    }

    // Update is called once per frame
    void Update()
    {
        // Activate gravity selection mode when Left Shift is held
        isGravityChangeMode = Input.GetKey(KeyCode.LeftShift);

        if (hologramInstance)
            hologramInstance.SetActive(isGravityChangeMode);

        if (isGravityChangeMode)
            HandleGravitySelection();

        if (Input.GetKeyDown(KeyCode.Return) && isGravityChangeMode)
            ApplyGravity();
    }

    //void FixedUpdate()
    //{
    //    rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    //}

    //void HandleGravitySelection()
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow))
    //        SelectGravity(Vector3.up);
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //        SelectGravity(Vector3.right);
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        SelectGravity(Vector3.left);
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //        SelectGravity(Vector3.down);
    //}

    void HandleGravitySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            positionIndex = (positionIndex + 1) % 5; // Cycle forward
            SelectGravity(hologramPositions[positionIndex].transform);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionIndex = (positionIndex - 1 + 5) % 5; // Cycle backward
            SelectGravity(hologramPositions[positionIndex].transform);
        }
    }


    void SelectGravity(Transform transform)
    {
        selectedGravity = (transform.position).normalized;

        if (hologramInstance)
        {
            hologramInstance.transform.position = transform.position;
            hologramInstance.transform.rotation = transform.rotation;
            //hologramInstance.transform.rotation = Quaternion.LookRotation(-selectedGravity);
        }
    }

    //void SelectGravity(Vector3 direction)
    //{
    //    selectedGravity = direction;

    //    if (hologramInstance)
    //    {
    //        hologramInstance.transform.position = transform.position + direction * 1.5f; // Offset in selected direction
    //        hologramInstance.transform.rotation = Quaternion.LookRotation(-selectedGravity); // Face the player
    //    }
    //}
    //void ApplyGravity()
    //{
    //    Debug.Log("Gravity applied to" + selectedGravity);
    //}

    void RotatePlayer(Vector3 direction)
    {
        // Determine the new rotation based on gravity direction
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        transform.rotation = newRotation;
    }

    void ApplyGravity()
    {
        RotatePlayer(selectedGravity);
        Physics.gravity = selectedGravity * gravityStrength;



        if (hologramInstance)
            hologramInstance.SetActive(false); // Hide after applying gravity
    }
}

//using UnityEngine;

//public class GravityController : MonoBehaviour
//{
//    public Rigidbody playerRigidbody;
//    public float rotationSpeed = 5f;
//    private Vector3 currentGravity = Vector3.down;

//    void Start()
//    {
//        if (playerRigidbody == null)
//            playerRigidbody = GetComponent<Rigidbody>();

//        Physics.gravity = currentGravity;
//    }

//    void Update()
//    {
//        HandleInput();
//        AlignToSurface();
//    }

//    void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.G)) // Change to downward gravity
//        {
//            SelectGravity(Vector3.down);
//            GetComponent<CharacterMovement>().SetGravity(Vector3.down);
//        }
//        if (Input.GetKeyDown(KeyCode.F)) // Change to upward gravity
//        {
//            ChangeGravity(Vector3.up);
//            GetComponent<CharacterMovement>().SetGravity(Vector3.up);
//        }
//        if (Input.GetKeyDown(KeyCode.L)) // Change to leftward gravity
//        {
//            ChangeGravity(Vector3.left);
//            GetComponent<CharacterMovement>().SetGravity(Vector3.left);
//        }
//        if (Input.GetKeyDown(KeyCode.R)) // Change to rightward gravity
//        {
//            ChangeGravity(Vector3.right);
//            GetComponent<CharacterMovement>().SetGravity(Vector3.right);
//        }
//    }

//    public void ChangeGravity(Vector3 newGravity)
//    {
//        currentGravity = newGravity;
//        Physics.gravity = newGravity;
//        RotatePlayer(newGravity);
//    }

//    void RotatePlayer(Vector3 gravityDirection)
//    {
//        Quaternion targetRotation = Quaternion.LookRotation(Vector3.Cross(transform.right, -gravityDirection), -gravityDirection);
//        StartCoroutine(SmoothRotation(targetRotation));
//    }

//    System.Collections.IEnumerator SmoothRotation(Quaternion targetRotation)
//    {
//        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
//        {
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
//            yield return null;
//        }
//    }

//    void AlignToSurface()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
//        {
//            Quaternion surfaceRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
//            transform.rotation = Quaternion.Slerp(transform.rotation, surfaceRotation, Time.deltaTime * rotationSpeed);
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("GravityZone"))
//        {
//            Vector3 newGravity = other.transform.up; // Adjust based on zone's direction
//            ChangeGravity(newGravity);
//        }
//    }
//}
