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
    public ItemData daggerItemData; 
    public GameObject craftedDagger = null; // Reference to track the crafted dagger

    // References to the crafting slots
    public CraftingItemSlot[] craftingItemSlots;

    private bool daggerCrafted = false;

    //--------------------------------------------------------------------------------------------------------------

    // Clears the crafting slots
    private void ClearCraftingSlots()
    {
        CraftingItemSlot1().ClearSlot();
        CraftingItemSlot2().ClearSlot();
    }

    //--------------------------------------------------------------------------------------------------------------
    private CraftingItemSlot CraftingItemSlot1()
    {
        return craftingItemSlots[0];
    } 

    private CraftingItemSlot CraftingItemSlot2()
    {
        return craftingItemSlots[1];
    }

    //--------------------------------------------------------------------------------------------------------------

    // Handle the item change in the crafting slots
    public void HandleSlotItemChange(ItemData oldItem, ItemData newItem)
    {

        CraftItemIfNeeded();
        ReturnItemToInventoryIfNeeded(oldItem);
        RemoveItemFromInventoryIfNeeded(newItem);
    }

    //--------------------------------------------------------------------------------------------------------------

    private void ReturnItemToInventoryIfNeeded(ItemData item)
    {
        if (item != null)
        {
            Inventory.Instance.AddItem(item);
        }
    }

    private void RemoveItemFromInventoryIfNeeded(ItemData item)
    {
        if (item != null)
        {
            Inventory.Instance.RemoveItem(item);
        }
    }

    private void CraftItemIfNeeded()
    {
        if (CraftingItemSlot1().IsSlotFilled() && CraftingItemSlot2().IsSlotFilled())
        {

            // Only check the combination once both slots are filled
            if (IsCraftingSlotsBothFilled(CraftingItemSlot1(), CraftingItemSlot2()))
            {
                CreateCraftItem(CraftingItemSlot1().GetItem(), CraftingItemSlot2().GetItem());
            }
            else
            {
                // Invalid combination, return items to their original positions
                Debug.Log("Invalid combination. Returning items.");
            }
        }

    }

    //--------------------------------------------------------------------------------------------------------------

    // Check if both crafting slots are filled

    private bool IsCraftingSlotsBothFilled(CraftingItemSlot slot1, CraftingItemSlot slot2)
    {
        return slot1.IsSlotFilled() && slot2.IsSlotFilled() && IsCraftingSlotsItemsCombinationValid(slot1.GetItem(), slot2.GetItem());
    }

    //--------------------------------------------------------------------------------------------------------------

    // Check if the combination of items in the crafting slots is valid

    private bool IsCraftingSlotsItemsCombinationValid(ItemData item1, ItemData item2)
    {
        return (item1.itemName == "Wood" && item2.itemName == "Stone") ||
                (item1.itemName == "Stone" && item2.itemName == "Wood");
    }

    //--------------------------------------------------------------------------------------------------------------

    // Create the crafted item
    public void CreateCraftItem(ItemData item1, ItemData item2)
    {
        Debug.Log("Crafting Dagger");
        ItemData daggerItem = GetDaggerItem();

        if (daggerItem != null && daggerItem.itemPrefab != null)
        {
            // Set the flag to prevent multiple daggers from being crafted
            daggerCrafted = true;

            Inventory.Instance.AddItem(daggerItem);

            // Mark the objective as complete
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.TrackObjective("Craft a dagger");
                Debug.Log("Objective 'Craft a dagger' marked as complete.");
            }
            else
            {
                Debug.LogError("ObjectiveManager.Instance is null!");
            }

           
            Debug.Log("Dagger crafted and placed in Inventory.");

            // Remove the items from the inventory

            Inventory.Instance.RemoveItem(CraftingItemSlot1().GetItem());
            Inventory.Instance.RemoveItem(CraftingItemSlot2().GetItem());

            ClearCraftingSlots();
        }
        else
        {
            Debug.LogError("Dagger item or prefab is missing.");
        }
    }

    //--------------------------------------------------------------------------------------------------------------

    // Getter for the dagger item data

    private ItemData GetDaggerItem()
    {
        return daggerItemData;
    }
}



