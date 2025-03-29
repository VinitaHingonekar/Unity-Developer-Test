using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int collectedCubes = 0;
    private bool hasWon;
    private bool timeOut = false;

    [Header("Cubes")]
    public int totalCubes = 5;

    [Header("UI")]
    public GameObject gameScreen;
    public GameObject endScreen;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timerText;

    [Header("Timer")]
    public float timeLimit = 120f; // 2 minutes
    private float timeRemaining;
    private bool gameOver = false;

    void Start()
    {
        timeRemaining = timeLimit;
        gameScreen.SetActive(true);
        if (endScreen)
            endScreen.SetActive(false);
    }

    void Update()
    {
        if (!gameOver)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timeOut = true;
                PlayerDeath();
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
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
        gameOver = true;
        ShowEndScreen();
    }

    public void ShowEndScreen()
    {
        gameScreen.SetActive(false);

        gameOver = true;
        endScreen.SetActive(true);

        if (hasWon)
        {
            resultText.text = "Congratulations! You Win!";
        }
        else
        {
            if (timeOut)
                resultText.text = "Time Out!     You Lost!";
            else
                resultText.text = "Game Over!    You Lost!";
        }
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
