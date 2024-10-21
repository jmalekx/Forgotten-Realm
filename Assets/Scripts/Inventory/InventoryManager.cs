using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Menu")]
    public GameObject inventoryMenu;
    public GameObject itemPanelPrefab;
    public GameObject itemPanelGrid;

    private PlayerInput playerInput;
    private InputAction inventoryAction;
    private bool isInventoryOpen = false;

    private Inventory inventory;


    void Start()
    {
        inventoryMenu.SetActive(false);
        inventory = Inventory.Instance;
        inventory.OnInventoryChanged += UpdateInventoryUI; // Subscribe to inventory updates

        UpdateInventoryUI();
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        inventoryAction = playerInput.Main.Inventory;
        OnEnable();
    }

    void OnEnable()
    {
        inventoryAction.Enable();
        inventoryAction.performed += ToggleInventory;
    }

    void OnDisable()
    {
        inventoryAction.performed -= ToggleInventory;
        inventoryAction.Disable();
    }

    void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryMenu.SetActive(isInventoryOpen);
        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
        }
    }

    // Updates the inventory UI based on current inventory items
    void UpdateInventoryUI()
    {
        // Clear existing item panels
        foreach (Transform child in itemPanelGrid.transform)
        {
            Destroy(child.gameObject);
        }

        // Create a new item panel for each item in the inventory
        foreach (ItemData item in inventory.items)
        {
            GameObject newItemPanel = Instantiate(itemPanelPrefab, itemPanelGrid.transform);
            ItemSlot itemSlot = newItemPanel.GetComponent<ItemSlot>();

            if (itemSlot != null)
            {
                itemSlot.UpdateSlot(item); // Update the slot with item data
            }
        }
    }
}
