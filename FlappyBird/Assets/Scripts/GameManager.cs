using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    // UI Panels
    public GameObject homeUI;
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public GameObject inGamePauseUI;
    public GameObject leaderboardUI;
    // public GameObject shopUI;

    // UI Elements
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI finalScoreText;

    // REF to the player
    public GameObject Player;

    // REF to the level generator
    [SerializeField] private LevelGenerator levelGenerator;

    // REF to the score manager
    [SerializeField] private ScoreManager scoreManager;

    private bool isGameOver = false;

    void Awake()
    {
        // Ensure singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ShowHomeUI();
    }

    // Home logic
    public void ShowHomeUI()
    {
        homeUI.SetActive(true);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        Player.SetActive(false);
        inGamePauseUI.SetActive(false);
        leaderboardUI.SetActive(false);
        //shopUI.SetActive(false);
    }

    public void StartGame()
    {
        homeUI.SetActive(false);
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        Player.SetActive(true);
        inGamePauseUI.SetActive(false);

        // Reset game state
        isGameOver = false;
        ScoreManager.Instance.ResetScore();
        UpdateCurrentScore(0);
        GameObject[] Pipes = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject Pipe in Pipes)
        {
            Destroy(Pipe);
        }
        GameObject[] PipesTrigger = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject Trigger in PipesTrigger)
        {
            Destroy(Trigger);
        }

        scoreManager.bIsScoreSubmitted = false;
        levelGenerator.ResetDifficulty();
        levelGenerator.bIsPlaying = true;
        levelGenerator.SpawnPipe();
    }

    // In-Game logic
    public void UpdateCurrentScore(int score)
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + score;
        }
    }

    // Game Over logic
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Time.timeScale = 0f; // Pause the game
            levelGenerator.timer = 0f; // Reset the timer for pipe spawn

            // Display final score and high score
            if (finalScoreText != null)
            {
                finalScoreText.text = "Your Score: " + ScoreManager.Instance.CurrentScore;
            }

            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + ScoreManager.Instance.HighScore;
                if (ScoreManager.Instance.CurrentScore > ScoreManager.Instance.HighScore)
                {
                    ScoreManager.Instance.SaveScore();
                }
            }

            levelGenerator.bIsPlaying = false;

            ShowGameOverUI();
        }
    }

    public void ShowGameOverUI()
    {
        homeUI.SetActive(false);
        inGameUI.SetActive(false);
        gameOverUI.SetActive(true);
        Player.SetActive(false);
        Player.transform.position = Vector3.zero;
        inGamePauseUI.SetActive(false);
    }

    public void ShowInGamePauseUI()
    {
        Time.timeScale = 0; // Pause the game
        inGamePauseUI.SetActive(true);
    }

    public void ShowLeaderboardUI()
    {
        homeUI.SetActive(false);
        leaderboardUI.SetActive(true);
    }

    /*
    public void ShowShopUI()
    {
        shopUI.SetActive(true);
    }
    */

    public void BackToGame()
    {
        Time.timeScale = 1f; // Resume the game
        inGamePauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        Player.transform.position = Vector3.zero;
        GameObject[] Pipes = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject Pipe in Pipes)
        {
            Destroy(Pipe);
        }
        GameObject[] PipesTrigger = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject Trigger in PipesTrigger)
        {
            Destroy(Trigger);
        }
        StartGame();
    }

    public void BackToHome()
    {
        Time.timeScale = 1f; // Resume the game
        ShowHomeUI();
    }

    public void TestButton()
    {
        Debug.Log("Clicked !");
    }
}
