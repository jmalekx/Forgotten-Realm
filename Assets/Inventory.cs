using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> items = new List<ItemData>();

    public event Action OnInventoryChanged; // Event to notify inventory changes

    public Slider HealthBar;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        items.Clear();
    }

//------------------------------------------------------------------------------------------------------------
    public void AddItem(ItemData itemToAdd)
    {
        bool itemExists = false;

        foreach (ItemData item in items)
        {
            if (item.itemName == itemToAdd.itemName) // check by item name
            {
                item.count += 1; // always add 1 to the count for each pickup
                itemExists = true;
                break;
            }
        }

        if (!itemExists)
        {
            itemToAdd.count = 1; // initialize count to 1 for a new item
            items.Add(itemToAdd); // add new item if it does not exist
            
        }
        if(itemToAdd.itemName == "Apple"){
            HealthBar.value +=20;
        }
       

        OnInventoryChanged?.Invoke(); // notify the UI to update
        Debug.Log(itemToAdd.itemName + " added to inventory. Total count: " + itemToAdd.count);
    }

    //------------------------------------------------------------------------------------------------------------
    public void RemoveItem(ItemData itemToRemove)
    {
        foreach (var item in items)
        {
            if (item.itemName == itemToRemove.itemName)
            {
                item.count -= itemToRemove.count;
                if (item.count <= 0)
                {
                    items.Remove(item);
                }
                break;
            }
        }

        OnInventoryChanged?.Invoke(); // notify the UI to update
        Debug.Log(itemToRemove.count + " " + itemToRemove.itemName + " removed from inventory.");
    }
}
