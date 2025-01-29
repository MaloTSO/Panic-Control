using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button tutorialButton;
    public Button newGameButton;
    public Button quitButton;
    public TMP_InputField pseudoInputField;

    private void Start()
    {
        tutorialButton.onClick.AddListener(OnTutorialButtonClicked);
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    public void OnTutorialButtonClicked()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButtonClicked()
    {
        SavePseudo();
        Application.Quit();
    }

    public void SavePseudo()
    {
        if (string.IsNullOrEmpty(pseudoInputField.text))
        {
            return;
        }
        string pseudo = pseudoInputField.text;
        PlayerPrefs.SetString("PlayerPseudo", pseudo);
        PlayerPrefs.Save();
    }

}
