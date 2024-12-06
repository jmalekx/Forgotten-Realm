using UnityEngine;
using UnityEngine.SceneManagement; // For managing scenes (if you want to load the main menu)
using UnityEngine.UI; // For UI elements

public class GamePause : MonoBehaviour
{
    public GameObject pauseMenuUI; // The pause menu UI (assigned in the Inspector)
    public Button resumeButton; // Button to resume the game (assigned in the Inspector)
    public Button quitButton; // Button to quit the game (assigned in the Inspector)
    public Button mainMenuButton; // Button to go to the main menu (assigned in the Inspector)

    private bool isPaused = false; // Tracks whether the game is paused or not

    void Start()
    {
        // Ensure the pause menu is hidden at the start of the game
        pauseMenuUI.SetActive(false);

        // Add listeners to the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

  void Update()
{
    // Log when a key is pressed
    if (Input.GetKeyDown(KeyCode.P))
    {
        Debug.Log("P key pressed");
    }

    // Toggle pause
    if (Input.GetKeyDown(KeyCode.P))
    {
        if (isPaused)
        {
            ResumeGame(); // If already paused, resume the game
        }
        else
        {
            PauseGame(); // If not paused, pause the game
        }
    }
}


    void PauseGame()
{
    isPaused = true;
    Time.timeScale = 0f;
    Debug.Log("Game Paused - Time.timeScale = " + Time.timeScale); // Debug log
    pauseMenuUI.SetActive(true);
}

void ResumeGame()
{
    isPaused = false;
    Time.timeScale = 1f;
    Debug.Log("Game Resumed - Time.timeScale = " + Time.timeScale); // Debug log
    pauseMenuUI.SetActive(false);
}


    void QuitGame()
    {
        Debug.Log("Quit Game Button Pressed");

        // Quit the game or return to the main menu (in the case of a build)
        Application.Quit();
    }

    void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu");

        // You can use SceneManager to load the main menu scene here
        // Make sure you have a scene named "MainMenu" or change this to your scene name
        SceneManager.LoadScene("MainMenu");
    }
}
