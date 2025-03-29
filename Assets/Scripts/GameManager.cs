using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int collectedCubes = 0;
    private bool hasWon;

    [Header("Cubes")]
    public int totalCubes = 5;

    [Header("UI")]
    public GameObject endScreen;
    public TextMeshProUGUI resultText;

    void Start()
    {
        if (endScreen)
            endScreen.SetActive(false);
    }

    public void CollectCube()
    {
        collectedCubes++;
        Debug.Log("Cubes Collected: " + collectedCubes);

        if (collectedCubes >= totalCubes)
        {
            hasWon = true;
            ShowEndScreen();
        }
    }

    public void PlayerDeath()
    {
        hasWon = false;
        ShowEndScreen();
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);

        if (hasWon)
            resultText.text = "Congratulations! You Win!";
        else
            resultText.text = "Game Over!       You Lost!";
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }    

    public void QuitGame()
    {
        Application.Quit();
    }
}
