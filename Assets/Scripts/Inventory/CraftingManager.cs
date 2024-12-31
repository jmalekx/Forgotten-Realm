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
    public GameObject craftedDagger = null; // Reference to track the crafted dagger

    // References to the crafting slots
    public CraftingItemSlot[] craftingItemSlots;

    private bool daggerCrafted = false;

    // Clears the crafting slots
    private void ClearCraftingSlots()
    {
        CraftingItemSlot1().ClearSlot();
        CraftingItemSlot2().ClearSlot();
    }


    private CraftingItemSlot CraftingItemSlot1()
    {
        return craftingItemSlots[0];
    } 

    private CraftingItemSlot CraftingItemSlot2()
    {
        return craftingItemSlots[1];
    }
    public void HandleSlotItemChange(ItemData oldItem, ItemData newItem)
    {

        // 1. Craft an item?
        CraftItemIfNeeded();
        ReturnItemToInventoryIfNeeded(oldItem);
        RemoveItemFromInventoryIfNeeded(newItem);
    }

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

            Debug.Log("2");

            // Only check the combination once both slots are filled
            if (IsCraftingSlotsCombinationValid(CraftingItemSlot1(), CraftingItemSlot2()))
            {
                CreateCraftItem(CraftingItemSlot1().GetItem(), CraftingItemSlot2().GetItem());
            }
            else
            {
                // Invalid combination, return items to their original positions
                Debug.Log("Invalid combination. Returning items.");
                //ReturnItemsToInventory();
                //ClearCraftingSlots();
            }
        }

    }

    private bool IsCraftingSlotsCombinationValid(CraftingItemSlot slot1, CraftingItemSlot slot2)
    {
        return slot1.IsSlotFilled() && slot2.IsSlotFilled() && IsCraftingSlotsItemsCombinationValid(slot1.GetItem(), slot2.GetItem());
    }

    private bool IsCraftingSlotsItemsCombinationValid(ItemData item1, ItemData item2)
    {
        return (item1.itemName == "Wood" && item2.itemName == "Stone") ||
                (item1.itemName == "Stone" && item2.itemName == "Wood");
    }

    public void CreateCraftItem(ItemData item1, ItemData item2)
    {
        Debug.Log("Crafting Dagger");
        ItemData daggerItem = GetDaggerItem();

        if (daggerItem != null && daggerItem.itemPrefab != null)
        {
            // Set the flag to prevent multiple daggers from being crafted
            daggerCrafted = true;

            Inventory.Instance.AddItem(daggerItem);

           
            Debug.Log("Dagger crafted and placed in Inventory.");

            Inventory.Instance.RemoveItem(CraftingItemSlot1().GetItem());
            Inventory.Instance.RemoveItem(CraftingItemSlot2().GetItem());

            ClearCraftingSlots();
        }
        else
        {
            Debug.LogError("Dagger item or prefab is missing.");
        }
    }

    private ItemData GetDaggerItem()
    {
        return daggerItemData;
    }
}



