using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu; // Assign your PauseMenu Panel in the Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Stops the game
        isPaused = true;
    }

    public void ResumeGame()
{
    Debug.Log("Resume button clicked!"); // Add this line
    pauseMenu.SetActive(false);
    Time.timeScale = 1f;
    isPaused = false;
}

public void LoadMainMenu()
{
    Debug.Log("Main Menu button clicked!"); // Add this line
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
}

}
