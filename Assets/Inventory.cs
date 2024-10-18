using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
<<<<<<< Updated upstream
    /*
     * ^^^ This line declares a static variable Instance of type Inventory.
     * This static variable will hold the single instance of the Inventory class, 
     * implementing the Singleton design pattern. This pattern ensures that 
     * there is only one instance of the Inventory class that can be accessed globally.
     */
    public List<Item> items = new List<Item> ();
=======
    public List<ItemData> items = new List<ItemData>();
>>>>>>> Stashed changes

    public event Action OnInventoryChanged; // Event to notify inventory changes

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

    //public void DropItem(Item itemToDrop)
    //{
    //    // Ensure item exists in the inventory
    //    Item itemInInventory = items.Find(item => item.name == itemToDrop.name);
    //    if (itemInInventory != null && itemInInventory.itemPrefab != null)
    //    {
    //        // Remove the item from the inventory
    //        RemoveItem(new Item(itemToDrop.name, 1, itemToDrop.itemPrefab));

    //        // Instantiate the item in the world
    //        Vector3 dropPosition = transform.position + transform.forward; // Drop in front of the player
    //        Instantiate(itemToDrop.itemPrefab, dropPosition, Quaternion.identity);

    //        Debug.Log("Object has been dropped: " + itemToDrop.name);

    //    }
    //    else
    //    {
    //        Debug.Log("Failed to drop item: " + itemToDrop.name + ". Item not found or prefab is null.");
    //    }
    //}

}
