using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu; //pauseMenu panel (assigned in inspector to panel UI element)
    private bool isPaused = false; //boolean to track if paused 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Turn pause on with 'p' key
        {
            Pause(); //call pause
        }
    }

    //pause method
    public void Pause()
    {
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Stops the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        isPaused = true;
    }

    //resume button method
    public void ResumeGame()
    {
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center
        Cursor.visible = false; // Hide the cursor
        isPaused = false;
    }

    //main menu button method
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale to normal
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make cursor visible 
        SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene
    }
}
