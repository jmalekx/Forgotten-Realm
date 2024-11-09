using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public Canvas popupCanvas;
    public TMP_Text popupText;
    private bool hasDisplayedDropHint = false;
    private bool hasDisplayedAppleHint = false;
    private bool hasDisplayedScrollHint = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            popupCanvas.enabled = false; //hide by default
        }
    }

    // Method to display hints
    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupCanvas.enabled = true;
        Invoke("HidePopup", 3f); //hide after 3s
    }

    private void HidePopup()
    {
        popupCanvas.enabled = false;
    }

    //specific item hints
    public void DisplayItemHint(string itemName)
    {
        if (!hasDisplayedDropHint)
        {
            ShowPopup("Press 'Q' to drop items");
            hasDisplayedDropHint = true;
        }

        if (itemName == "Apple" && !hasDisplayedAppleHint)
        {
            ShowPopup("Press right-click to consume");
            hasDisplayedAppleHint = true;
        }
        else if (itemName == "Scroll" && !hasDisplayedScrollHint)
        {
            ShowPopup("Press right-click to view objectives");
            hasDisplayedScrollHint = true;
        }
    }
}
