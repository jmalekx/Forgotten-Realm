using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    public TextMeshProUGUI gemCountText; // Reference to the UI Text

    private int gemCount = 0; // Tracks the number of gems collected

    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Property to access the gem count
    public int GemCount
    {
        get { return gemCount; }
    }

    // Method to increment gem count
    public void CollectGem()
    {
        gemCount++; // Increase the gem count
        UpdateGemUI(); // Update the UI
    }

    // Update the UI to display the current gem count
    private void UpdateGemUI()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems Collected: " + gemCount;
        }
    }
}
