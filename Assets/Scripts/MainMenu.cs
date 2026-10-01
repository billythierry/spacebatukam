using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UIDocument uiDocument;
    private Label title;
    private Button startButton;
    private Button quitButton;
    public Button restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        title = uiDocument.rootVisualElement.Q<Label>("TitleLabel");
        startButton = uiDocument.rootVisualElement.Q<Button>("StartButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitButton");

        startButton.clicked += StartGame;
        quitButton.clicked += QuitGame;

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
