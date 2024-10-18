using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public 
    class Inventory : MonoBehaviour

{
    public static Inventory Instance;
    public List<Item> items = new List<Item> ();

    public Slider HealthBar;
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
        HealthBar.value +=30;
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
}
