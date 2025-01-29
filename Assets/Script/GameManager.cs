using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;
    private bool gameRunning;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }
    public void GameOver()
    {
        gameRunning = false;
        gameOverPanel.SetActive(true);
    }

    public void StartGame()
    {
        gameRunning = true;
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
