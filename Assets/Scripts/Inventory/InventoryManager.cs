using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Menu")]
    public GameObject inventoryMenu;
    public GameObject itemPanel;
    public GameObject itemPanelGrid;

    private PlayerInput playerInput;
    private InputAction inventoryAction;
    private bool isInventoryOpen = false;


    void Start()
    {
        inventoryMenu.SetActive(false);
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
}
