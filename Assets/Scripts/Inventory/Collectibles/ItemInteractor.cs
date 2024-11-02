using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 3.0f; //interaction range
    public GameObject itemPromptPrefab; //prefab for text

    private GameObject currentPrompt; //current prompt
    private Collectible currentItem; //collectible
    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = new PlayerInput();
        interactAction = playerInput.Main.Interact;

    }

    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.performed += OnPickup;
    }

    private void OnDisable()
    {
        interactAction.performed -= OnPickup;
        interactAction.Disable();
    }
    void Update()
    {
        //raycast from camera center
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            //if object is collectible
            Collectible collectible = hit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                if (currentItem != collectible)
                {
                    currentItem = collectible;
                    ShowItemPrompt(collectible.collectibleItemData.itemName, collectible.transform.position);
                }
            }
            else
            {
                HideItemPrompt();
                currentItem = null;
            }
        }
        else
        {
            HideItemPrompt();
            currentItem = null;
        }

        //if prompt active, rotate to player camera
        if (currentPrompt != null)
        {
            FacePlayer();
        }
    }

    void OnPickup(InputAction.CallbackContext context)
    {
        if (currentItem != null)
        {
            PickupItem();
        }
    }
    void PickupItem()
    {
        Inventory.Instance.AddItem(currentItem.collectibleItemData); //add item
        Destroy(currentItem.gameObject); //destroy collectible object
        currentItem = null; //clear current item
        HideItemPrompt(); 
    }
    private void ShowItemPrompt(string itemName, Vector3 position)
    {
        if (currentPrompt == null)
        {
            //if prompt doesnt exist
            currentPrompt = Instantiate(itemPromptPrefab, position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        }

        //access components and update
        TMP_Text dynamicText = currentPrompt.transform.Find("dynamic").GetComponent<TMP_Text>();
        TMP_Text staticText = currentPrompt.transform.Find("static").GetComponent<TMP_Text>();
        TMP_Text customText = currentPrompt.transform.Find("customDescription").GetComponent<TMP_Text>();

        if (dynamicText != null)
        {
            dynamicText.text = $"{itemName}"; //update to item name
        }
        if (staticText != null)
        {
            staticText.text = "E to pickup item"; //text that never changes - if do e to pickup can change this to that
        }
        if (customText != null)
        {
            customText.text = currentItem.collectibleItemData.customDescription; //custom item desc 
        }

        //update promp above text
        currentPrompt.transform.position = position + new Vector3(0, 1.1f, 0);
    }

    private void HideItemPrompt()
    {
        if (currentPrompt != null)
        {
            Destroy(currentPrompt); 
            currentPrompt = null; 
        }
    }

    //make prompt face player camera
    private void FacePlayer()
    {
        //rotate
        currentPrompt.transform.LookAt(currentPrompt.transform.position + playerCamera.transform.forward);
    }
}
