using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu; // Assign your PauseMenu Panel in the Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Toggle pause with 'P' key
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Stops the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("Resume button clicked!"); // Log for debugging
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center
        Cursor.visible = false; // Hide the cursor
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked!"); // Log for debugging
        Time.timeScale = 1f; // Reset time scale to normal
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Ensure cursor visibility
        SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene
    }
}
