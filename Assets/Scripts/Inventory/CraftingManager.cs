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
    public ItemSlot[] craftingSlots;


    // Track items in the crafting slots
    private ItemData itemInSlot1 = null;
    private ItemData itemInSlot2 = null;
    //private ItemData itemInSlot3 = null;

    // References to the crafting slots
    public CraftingItemSlot craftingSlot1;
    public CraftingItemSlot craftingSlot2;
    //public ItemSlot craftingSlot3;


    private bool daggerCrafted = false;

    //public void OnMouseDownItem(ItemData item)
    //{
    //    if (currentItem == null)
    //    {
    //        currentItem = item;

    //    }
    //}

    // Clears the crafting slots
    private void ClearCraftingSlots()
    {
        itemInSlot1 = null;
        itemInSlot2 = null;
        //itemInSlot3 = null;

        craftingSlot1.ClearSlot();
        craftingSlot2.ClearSlot();
        //craftingSlot3.ClearSlot();

    }

    public void HandleSlotItemChange()
    {

        Debug.Log("1");

        // 1. Are both slots filled?

        if (craftingSlot1.IsSlotFilled() && craftingSlot2.IsSlotFilled())
        {

            Debug.Log("2");

            Debug.Log("Item in slot 1: " + itemInSlot1);
            Debug.Log("Item in slot 2: " + itemInSlot2);

            // If the targetSlot is empty, assign the dropped item to it

            Debug.Log("3");

            //targetSlot.UpdateSlot(droppedItem);
            //TrackDroppedItem(droppedItem, targetSlot);
            //craftingSlot3.ClearSlot(); // Clear slot 3 if the dagger is dragged

            // Only check the combination once both slots are filled
            if (IsCraftingSlotsCombinationValid(craftingSlot1, craftingSlot2))
            {
                CreateCraftItem(itemInSlot1, itemInSlot2);
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

    private void TrackDroppedItem(ItemData droppedItem, ItemSlot targetSlot)
    {
        // Track the items in the crafting slots
        if (targetSlot == craftingSlot1)
        {
            itemInSlot1 = droppedItem;
        }
        else if (targetSlot == craftingSlot2)
        {
            itemInSlot2 = droppedItem;
        }
    }


    // Check if the combination of two items is valid
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

            ClearCraftingSlots();
            Debug.Log("Dagger crafted and placed in Inventory.");

            Inventory.Instance.RemoveItem(craftingSlot1.GetItem());
            Inventory.Instance.RemoveItem(craftingSlot2.GetItem());
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



