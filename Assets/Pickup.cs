using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    //public Item item = new Item("Item name", 1, ); //create instance of item

    // Assign the prefab directly in the inspector
    public GameObject itemPrefab;

    // Initialize the item with the item's name, count, and prefab
    public Item item;

    private void Awake()
    {
        // Create instance of item
        item = new Item("Item name", 1, itemPrefab);
    }

    private void OnTriggerEnter(Collider other) 

    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
