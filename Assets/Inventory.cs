using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public 
    class Inventory : MonoBehaviour

{
    public static Inventory Instance;
    /*
     * ^^^ This line declares a static variable Instance of type Inventory.
     * This static variable will hold the single instance of the Inventory class, 
     * implementing the Singleton design pattern. This pattern ensures that 
     * there is only one instance of the Inventory class that can be accessed globally.
     */
    public List<Item> items = new List<Item> ();


    void Awake()
    {
        if (Instance != null)
        Destroy(gameObject);
        else
            Instance = this;
    }


    public void AddItem(Item itemToAdd)
    {
        bool itemExists = false;

        foreach (Item item in items)
        {
            if(item.name == itemToAdd.name)
            {
                item.count += itemToAdd.count;
                itemExists = true;
                break;
            }
        }
        if (!itemExists)
        {
            items.Add(itemToAdd);
        }
        Debug.Log(itemToAdd.count + " " + itemToAdd.name + "added to inventory.");
    }

    public void RemoveItem(Item itemToRemove)
    {
        foreach (var item in items)
        {
            if(item.name == itemToRemove.name)
            {
                item.count -= itemToRemove.count;
                if(item.count == 0)
                {
                    items.Remove(itemToRemove);
                }
                break;
            }
        }
        Debug.Log(itemToRemove.count + " " + itemToRemove.name + "removed from inventory.");
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
