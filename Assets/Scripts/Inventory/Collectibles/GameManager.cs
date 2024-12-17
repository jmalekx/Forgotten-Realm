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

    public void CollectGem()
    {
        gemCount++; // Increase the gem count
        UpdateGemUI(); // Update the UI
    }

    private void UpdateGemUI()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems Collected: " + gemCount;
        }
    }
}
