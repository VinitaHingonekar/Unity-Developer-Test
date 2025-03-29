using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    public GameManager gameManager;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has fallen into the void!");
            other.gameObject.SetActive(false);
            gameManager.PlayerDeath();
        }
    }
}
