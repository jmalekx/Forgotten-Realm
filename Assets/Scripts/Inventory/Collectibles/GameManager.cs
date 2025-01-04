using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI gemCountText; // Reference to the UI Text
    public int gemsNeededToComplete = 10; // Number of gems needed to complete the objective

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

        // Track the objective when gemsNeededToComplete is reached
        if (gemCount >= gemsNeededToComplete)
        {
            TrackObjective("Collect all 10 gems to collect final scroll");
        }
    }

    // Update the UI to display the current gem count
    private void UpdateGemUI()
    {
        if (gemCountText != null)
        {
            gemCountText.text = "Gems Collected: " + gemCount;
        }
    }

    // Method to track the objective
    private void TrackObjective(string objectiveDescription)
    {
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective(objectiveDescription);
            Debug.Log("Objective Completed: " + objectiveDescription);
        }
    }
}
