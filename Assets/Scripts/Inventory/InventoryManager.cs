using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Inventory Manager class to handle inventory UI and input

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Menu")]
    public GameObject inventoryMenu;
    public GameObject itemPanelPrefab;
    public GameObject itemPanelGrid;
    public TextMeshProUGUI holdingText;

    [Header("Audio")]
    public AudioClip dropSound;
    public AudioClip eatSound;
    public AudioClip paperSound;
    private AudioSource audioSource;

    public Camera playerCamera;

    private PlayerInput playerInput;
    private InputAction scrollAction;
    private InputAction dropAction;
    private InputAction useAction;
    private InputAction selectSlotAction;

    private Inventory inventory;
    public int selectedItemIndex = 0;

    private PopupAnim objectivePopupAnim;
    private Coroutine holdingTextCoroutine; 

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnInventoryChanged += UpdateInventoryUI;
        UpdateInventoryUI();

        objectivePopupAnim = GameObject.Find("objectiveCanvas").GetComponent<PopupAnim>();
        objectivePopupAnim.gameObject.SetActive(false);
        holdingText.gameObject.SetActive(false);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        scrollAction = playerInput.UI.ScrollWheel;
        dropAction = playerInput.Main.Drop;
        useAction = playerInput.Main.Use;
        selectSlotAction = playerInput.Main.SelectSlot;

        OnEnable();
    }

    void OnEnable()
    {
        scrollAction.Enable();
        dropAction.Enable();
        useAction.Enable();
        selectSlotAction.Enable();

        scrollAction.performed += OnScroll;
        dropAction.performed += OnDropItem;
        useAction.performed += OnUseItem;
        useAction.canceled += OnUseItemReleased;
        selectSlotAction.performed += OnSelectSlot;
    }

    void OnDisable()
    {
        scrollAction.performed -= OnScroll;
        dropAction.performed -= OnDropItem;
        useAction.performed -= OnUseItem;
        selectSlotAction.performed -= OnSelectSlot;

        scrollAction.Disable();
        dropAction.Disable();
        selectSlotAction.Disable();
    }

    void OnSelectSlot(InputAction.CallbackContext context)
    {
        //map control to slot index
        string keyPressed = context.control.name;
        int slot = keyPressed switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            "5" => 4,
            "6" => 5,
            "7" => 6,
         
            _ => -1, //handle invalid
        };

        if (slot >= 0 && slot < 7) //inventory range
        {
            selectedItemIndex = slot;
            UpdateInventoryUI();
        }
    }

    void ShowHoldingText(string itemName)
    {
        //if coroutine already running stop to reset timer
        if (holdingTextCoroutine != null)
        {
            StopCoroutine(holdingTextCoroutine);
        }

        holdingText.text = $"Holding: {itemName}";
        holdingText.gameObject.SetActive(true);

        //start new
        holdingTextCoroutine = StartCoroutine(FadeHoldingText());
    }

    IEnumerator FadeHoldingText()
    {
        yield return new WaitForSeconds(2f);
        holdingText.gameObject.SetActive(false); //hide after 3s
    }

    //---------------------------------------------------------------------------------------------------

    void OnUseItem(InputAction.CallbackContext context)
    {
        if (inventory.items.Count > 0 && selectedItemIndex >= 0 && selectedItemIndex < inventory.items.Count)
        {
            ItemData selectedItem = inventory.items[selectedItemIndex];
            if (selectedItem.isConsumable)
            {
                audioSource.PlayOneShot(eatSound, 1.6f);
                inventory.UseItem(selectedItem);
                UpdateInventoryUI();
            }
            if (selectedItem.itemName == "Scroll")
            {
                audioSource.PlayOneShot(paperSound, 1f);
                objectivePopupAnim.ShowPopup(); //show objectives while pressed
            }
        }
        else
        {
            Debug.LogWarning("Invalid selected item index or empty inventory.");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnUseItemReleased(InputAction.CallbackContext context)
    {
        if (selectedItemIndex >= 0 && selectedItemIndex < inventory.items.Count)
        {
            ItemData selectedItem = inventory.items[selectedItemIndex];
            if (selectedItem.itemName == "Scroll")
            {
                //hide popup when right-click is released
                objectivePopupAnim.HidePopup();
            }
        }
        else
        {
            Debug.LogWarning("Invalid selected item index on release.");
        }
    }

    //---------------------------------------------------------------------------------------------------

    void UpdateInventoryUI()
    {
        //clear existing item panels
        foreach (Transform child in itemPanelGrid.transform)
        {
            Destroy(child.gameObject);
        }

        //create new item panel for each item in the inventory
        int maxSlots = 7;
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject newItemPanel = Instantiate(itemPanelPrefab, itemPanelGrid.transform);
            ItemSlot itemSlot = newItemPanel.GetComponent<ItemSlot>();

            if (itemSlot != null)
            {
                if (i < inventory.items.Count)
                {
                    itemSlot.UpdateSlot(inventory.items[i]);

                    //update text for selecteditem to be displayed
                    if (i == selectedItemIndex)
                    {
                        ShowHoldingText(inventory.items[i].itemName);
                    }

                }
                else
                {
                    itemSlot.ClearSlot();

                }
                itemSlot.SetSelected(i == selectedItemIndex);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    private void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        int maxSlots = 7;
        if (scrollValue.y > 0)
        {
            selectedItemIndex = (selectedItemIndex - 1 + maxSlots) % maxSlots; //scroll up
        }
        else if (scrollValue.y < 0)
        {
            selectedItemIndex = (selectedItemIndex + 1) % maxSlots; //scroll down
        }

        UpdateInventoryUI();
    }

    //---------------------------------------------------------------------------------------------------

    // Drop the selected item from the inventory into the world
    private void OnDropItem(InputAction.CallbackContext context)
    {
        if (inventory.items.Count > 0)
        {
            //debug log the inventory contents

            Debug.Log("Dropping item at index: " + selectedItemIndex);

            ItemData selectedItem = inventory.items[selectedItemIndex];

            if (selectedItem.count > 0)
            {
                //decrease the item count by one
                Inventory.Instance.DecrementAvailableItem(selectedItem);
                UpdateInventoryUI();

                //if the count reaches zero remove the item from inventory
                if (selectedItem.count <= 0)
                {
                    inventory.RemoveItem(selectedItem);
                }

                audioSource.PlayOneShot(dropSound, 0.65f);
                //place the item back into the world
                DropItemToWorld(selectedItem);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------

    private void DropItemToWorld(ItemData item)
    {
        if (item.itemPrefab != null)
        {
            //drop position in front of the player camera
            Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 3.5f;
            GameObject droppedItem = Instantiate(item.itemPrefab, dropPosition, Quaternion.identity);

            //rb attached and config
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = droppedItem.AddComponent<Rigidbody>();
            }

            rb.isKinematic = false;
            rb.AddForce(playerCamera.transform.up * 5f, ForceMode.Impulse); //upward force

            CraftingManager craftingManager = droppedItem.GetComponent<CraftingManager>(); //add crafting manager
            if (craftingManager == null)
            {
                craftingManager = droppedItem.AddComponent<CraftingManager>(); 
            }

    
            Debug.Log("Dropped " + item.itemName + " into the world.");
        }
        else
        {
            Debug.LogError("Item prefab is null. Cannot drop item into the world.");
        }
    }

    //---------------------------------------------------------------------------------------------------

    public void AddItemToInventory(ItemData item)
    {
 
        inventory.items.Add(item);
        UpdateInventoryUI(); 
    }

    public void RemoveItemFromInventory(ItemData item)
    {
 
        inventory.items.Remove(item);
        UpdateInventoryUI(); 
    }


}