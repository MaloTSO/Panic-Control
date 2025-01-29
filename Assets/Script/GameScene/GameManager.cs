using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI pseudoText;

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
        string pseudo = PlayerPrefs.GetString("PlayerPseudo");
        if (!string.IsNullOrEmpty(pseudo))
        {
            pseudoText.text = pseudo;
        }
        else
        {
            pseudoText.text = "Player";
        } 
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
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
