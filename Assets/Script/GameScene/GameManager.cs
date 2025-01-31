using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI pseudoText;
    [SerializeField] Image heartRateBarBackground;

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
        Time.timeScale = 1;
        string pseudo = PlayerPrefs.GetString("PlayerPseudo");
        heartRateBarBackground.gameObject.SetActive(true);
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
        heartRateBarBackground.gameObject.SetActive(false);
        
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        gameRunning = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
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
