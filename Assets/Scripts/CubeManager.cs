using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Cube Collected!");
            FindObjectOfType<GameManager>().CollectCube();
            Destroy(gameObject);
        }
    }
}
