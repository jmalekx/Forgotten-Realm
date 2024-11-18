using UnityEngine;
using TMPro;

[RequireComponent(typeof(PopupAnim))]
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    private TMP_Text popupText;
    private PopupAnim popupAnim;

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
            popupText = GetComponentInChildren<TMP_Text>();
            popupAnim = GetComponent<PopupAnim>();

            gameObject.SetActive(false);
        }
    }

    // Method to display hints
    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupAnim.ShowPopup();
        Invoke(nameof(HidePopup), 3f); //hide after 3s
    }

    private void HidePopup()
    {
        popupAnim.HidePopup();
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
