using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    public GameObject jetFlame;
    public GameObject explosionEffect;
    public GameObject borderParent;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;
    private Button backToMenuButton;
    private Button resumeButton;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        backToMenuButton = uiDocument.rootVisualElement.Q<Button>("BackToMenuButton");
        resumeButton = uiDocument.rootVisualElement.Q<Button>("ResumeButton");
  
        restartButton.style.display = DisplayStyle.None; // Hide the restart button initially
        backToMenuButton.style.display = DisplayStyle.None; // Hide the back to menu button initially
        resumeButton.style.display = DisplayStyle.None; // Hide the resume button initially

        restartButton.clicked += RestartGame;
        backToMenuButton.clicked += BackToMenu;
        resumeButton.clicked += ResumeGame;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        PlayerMove();
        JetFlameAnim();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);

        borderParent.SetActive(false); // Hide the parent border when the player is destroyed

        restartButton.style.display = DisplayStyle.Flex; 
        backToMenuButton.style.display = DisplayStyle.Flex;
    }

    public void PlayerMove()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            // Calculate mouse direction
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            //Limit player speed
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    public void JetFlameAnim()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            jetFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            jetFlame.SetActive(false);
        }
    }

    public void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score : " + score;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        isPaused = false; 
        Time.timeScale = 1f;
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze the game
        backToMenuButton.style.display = DisplayStyle.Flex; // Show the back to menu button when the game is paused
        resumeButton.style.display = DisplayStyle.Flex; // Show the resume button when the game is paused
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Freeze the game
        resumeButton.style.display = DisplayStyle.None; // Hide the resume button when the game is resumed
        backToMenuButton.style.display = DisplayStyle.None; // Hide the back to menu button when the game is resumed
    }
}
