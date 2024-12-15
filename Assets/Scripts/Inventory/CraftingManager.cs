using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using TMPro;


public class CraftingManager : MonoBehaviour
{
    public ItemData currentItem;
    public ItemData daggerItemData; // Reference to the dagger's ItemData (assign in inspector)
    private GameObject craftedDagger = null; // Reference to track the crafted dagger



    public Image customCursor;
    public ItemSlot[] craftingSlots;

    private Vector2 originalPosition;
    private RectTransform draggingRectTransform;

    // Track items in the crafting slots
    private ItemData itemInSlot1 = null;
    private ItemData itemInSlot2 = null;
    private ItemData itemInSlot3 = null;

    // References to the crafting slots
    public ItemSlot craftingSlot1;
    public ItemSlot craftingSlot2;
    public ItemSlot craftingSlot3;


    private bool daggerCrafted = false;




    public void OnMouseDownItem(ItemData item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.GetComponent<Image>().sprite;
            originalPosition = item.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    // Method to handle returning items to inventory if the combination is invalid
    private void ReturnItemsToInventory()
    {
        if (itemInSlot1 != null)
        {
            // Return itemInSlot1 to the inventory (implement as per your game logic)
            Debug.Log("Returning item1 to inventory: " + itemInSlot1.itemName);
        }
        if (itemInSlot2 != null)
        {
            // Return itemInSlot2 to the inventory (implement as per your game logic)
            Debug.Log("Returning item2 to inventory: " + itemInSlot2.itemName);
        }
        // Clear the crafting slots
        ClearCraftingSlots();
    }

    // Clears the crafting slots
    public void ClearCraftingSlots()
    {
        itemInSlot1 = null;
        itemInSlot2 = null;
        
        craftingSlot1.ClearSlot();
        craftingSlot2.ClearSlot();

    }

    public void OnItemDropped(ItemData droppedItem, ItemSlot targetSlot)
    {
        if (droppedItem != null)
        {
            // If the targetSlot is empty, assign the dropped item to it
            if (targetSlot.item == null)
            {
                targetSlot.UpdateSlot(droppedItem);

                // Track the items in the crafting slots
                if (targetSlot == craftingSlot1)
                {
                    itemInSlot1 = droppedItem;
                }
                else if (targetSlot == craftingSlot2)
                {
                    itemInSlot2 = droppedItem;
                }
                else if (targetSlot == craftingSlot3)
                {
                    // If a dagger is dropped into the inventory or another slot, clear crafting slot 3
                    if (craftedDagger != null && droppedItem == craftedDagger.GetComponent<ItemData>())
                    {
                        craftingSlot3.ClearSlot(); // Clear slot 3 if the dagger is dragged
                        craftedDagger = null; // Reset craftedDagger reference
                        Debug.Log("Dagger removed from Craft Slot 3 and added to inventory.");
                    }
                }

                // Only check the combination once both slots are filled
                if (itemInSlot1 != null && itemInSlot2 != null)
                {
                    // Check if the combination is valid
                    bool validCombination = CheckForValidCombination(itemInSlot1, itemInSlot2);
                    if (validCombination)
                    {
                        // Process the crafting result (e.g., create dagger)
                        CraftItem(itemInSlot1, itemInSlot2);

                        // Clear the crafting slots after confirming a valid combination
                        ClearCraftingSlots();
                    }
                    else
                    {
                        // Invalid combination, return items to their original positions
                        Debug.Log("Invalid combination. Returning items.");
                        ReturnItemsToInventory();
                    }
                }
                else
                {
                    Debug.Log("Both crafting slots must be occupied before crafting.");
                }
            }
        }
    }


    // Check if the combination of two items is valid
    public bool CheckForValidCombination(ItemData item1, ItemData item2)
    {
        return (item1.itemName == "Wood" && item2.itemName == "Stone") ||
               (item1.itemName == "Stone" && item2.itemName == "Wood");
    }

    public void CraftItem(ItemData item1, ItemData item2)
{
    Debug.Log("Crafting Dagger");

    // Check if craftingSlot3 is assigned
    if (craftingSlot3 != null)
    {
        // Ensure only one dagger is crafted
        if (!daggerCrafted)
        {
            // Get the dagger ItemData (make sure you have a valid reference to it)
            ItemData daggerItem = GetDaggerItem();

            if (daggerItem != null && daggerItem.itemPrefab != null)
            {
                // Instantiate the dagger prefab in craftingSlot3 position
                GameObject craftedDagger = Instantiate(daggerItem.itemPrefab, craftingSlot3.transform.position, Quaternion.identity);

                // Optionally, if you want to update the slot with the crafted dagger item
                craftingSlot3.UpdateSlot(daggerItem);

                // Set the flag to prevent multiple daggers from being crafted
                daggerCrafted = true;

                Debug.Log("Dagger crafted and placed in Slot 3.");
            }
            else
            {
                Debug.LogError("Dagger item or prefab is missing.");
            }
        }
        else
        {
            Debug.Log("A dagger has already been crafted. No new dagger created.");
        }
    }
    else
    {
        Debug.LogError("Crafting Slot 3 is not assigned.");
    }
}


    // Method to get the dagger ItemData (this is just an example, adjust to your game logic)
    private ItemData GetDaggerItem()
    {
        // You should return the actual ItemData for the dagger.
        // For example, if you have a predefined dagger ItemData:
        return daggerItemData;
    }


    // Method to get CraftingSlot1
    public ItemSlot GetCraftingSlot1()
    {
        return craftingSlot1;
    }

    // Method to get CraftingSlot2
    public ItemSlot GetCraftingSlot2()
    {
        return craftingSlot2;
    }


    // Method to handle when the dagger is dragged and dropped into the inventory panel
    public void OnDaggerDroppedIntoInventory()
    {
        if (craftedDagger != null)
        {
            // Clear craftingSlot3 when the dagger is placed into the inventory
            craftedDagger.GetComponent<DraggableItem>().enabled = false;  // Disable dragging of the dagger
            craftedDagger = null;  // Reset the dagger reference
            craftingSlot3.ClearSlot(); // Clear the crafting slot

            Debug.Log("Dagger moved to inventory and crafting slot 3 cleared.");
        }
    }
}



