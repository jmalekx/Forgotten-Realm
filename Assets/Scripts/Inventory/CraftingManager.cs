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

    private Vector2 originalPosition;
    private RectTransform draggingRectTransform;
   
    // Track items in the crafting slots
    private ItemData itemInSlot1 = null;
    private ItemData itemInSlot2 = null;
    //private ItemData itemInSlot3 = null;

    // References to the crafting slots
    public ItemSlot craftingSlot1;
    public ItemSlot craftingSlot2;
    //public ItemSlot craftingSlot3;


    private bool daggerCrafted = false;
    
    //public void OnMouseDownItem(ItemData item)
    //{
    //    if (currentItem == null)
    //    {
    //        currentItem = item;
            
    //    }
    //}

    // Method to handle returning items to inventory if the combination is invalid
    private void ReturnItemsToInventory()
    {
        if (itemInSlot1 != null)
        {
            Debug.Log("Returning item1 to inventory: " + itemInSlot1.itemName);
        }
        if (itemInSlot2 != null)
        {
            Debug.Log("Returning item2 to inventory: " + itemInSlot2.itemName);
        }

        ClearCraftingSlots();
    }

    // Clears the crafting slots
    public void ClearCraftingSlots()
    {
        itemInSlot1 = null;
        itemInSlot2 = null;
        //itemInSlot3 = null;

        craftingSlot1.ClearSlot();
        craftingSlot2.ClearSlot();
        //craftingSlot3.ClearSlot();

    }

    public void OnItemDropped(ItemData droppedItem, ItemSlot targetSlot)
    {

        if (droppedItem != null)
        {
            // If the targetSlot is empty, assign the dropped item to it

            if (targetSlot.isSlotEmpty())
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
                //else if (targetSlot == craftingSlot3)
                //{
                //    itemInSlot3 = droppedItem;
                   



                //    // If a dagger is dropped into the inventory or another slot, clear crafting slot 3
                //    //if (craftedDagger != null && droppedItem == craftedDagger.GetComponent<ItemData>())
                //    //{
                //    //    //Inventory.Instance.AddItem(droppedItem); // Add the dagger to the inventory

                //    //    //craftedDagger = null; // Reset craftedDagger reference


                //    //    Debug.Log("Dagger removed from Craft Slot 3 and added to inventory.");
                //    //}
                //}

                //craftingSlot3.ClearSlot(); // Clear slot 3 if the dagger is dragged

                // Only check the combination once both slots are filled
                if (itemInSlot1 != null && itemInSlot2 != null)
                {
                    bool validCombination = CheckForValidCraftingSlotsCombination(itemInSlot1, itemInSlot2);
                    if (validCombination)
                    {
                        CraftItem(itemInSlot1, itemInSlot2);
                        

                    }
                    else
                    {
                        // Invalid combination, return items to their original positions
                        Debug.Log("Invalid combination. Returning items.");
                        ReturnItemsToInventory();
                        //ClearCraftingSlots();
                    }
                }
                else
                {
                    Debug.Log("Both crafting slots must be occupied before crafting.");
                }



            }
           

        }

        Inventory.Instance.RemoveItem(droppedItem);


    }


    // Check if the combination of two items is valid
    public bool CheckForValidCraftingSlotsCombination(ItemData item1, ItemData item2)
    {
        return (item1.itemName == "Wood" && item2.itemName == "Stone") ||
               (item1.itemName == "Stone" && item2.itemName == "Wood");
    }

    public void CraftItem(ItemData item1, ItemData item2)
{
    Debug.Log("Crafting Dagger");

    
            
     
            ItemData daggerItem = GetDaggerItem();

            if (daggerItem != null && daggerItem.itemPrefab != null)
            {

                    //Inventory.Instance.AddItem(daggerItem);

                    //craftingSlot3.UpdateSlot(daggerItem);



                    // Set the flag to prevent multiple daggers from being crafted
                    daggerCrafted = true;

                    Inventory.Instance.AddItem(daggerItem);

                    ClearCraftingSlots();  
                    Debug.Log("Dagger crafted and placed in Slot 3.");
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


    public ItemSlot GetCraftingSlot1()
    {
        return craftingSlot1;
    }

    public ItemSlot GetCraftingSlot2()
    {
        return craftingSlot2;
    }


    public void onCraftingSlotItemUpdated()
    {
        // move here the logic from ItemSlog.OnDrop
    }
}



