using UnityEngine;
using TMPro;

[RequireComponent(typeof(PopupAnim))]
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public int displayTime = 3;

    [Header("Audio")]
    public AudioClip popupSound;
    public AudioSource audioSource;

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

    //method to display hints
    public void ShowPopup(string message)
    {

        //ensure popup canvas active before showing (to prevent corouritne error)
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        popupText.text = message;
        popupAnim.ShowPopup();
        PlayPopupSound();
        Invoke(nameof(HidePopup), displayTime); // hide after 3s
    }
    private void PlayPopupSound()
    {
        if (popupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(popupSound, 0.5f);
        }
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
