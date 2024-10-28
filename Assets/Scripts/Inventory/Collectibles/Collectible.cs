/*
 * Collectible class: Handles the interaction when the player collides with a collectible item.
 * - Contains a reference to the item's data (ItemData).
 * - OnTriggerEnter detects when the player collides with the item.
 * - If the player collides, it adds the item to the inventory and destroys the collectible object.
 */


using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemData collectibleItemData; // Reference to the item's data

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collectible: OnTriggerEnter called"); // Log when the trigger is activated
    //    if (other.CompareTag("Player")) // Check if the colliding object is tagged as "Player"
    //    {
    //        Debug.Log("Player collided with collectible: " + collectibleItemData.itemName); // Log the item's name
    //        Inventory.Instance.AddItem(collectibleItemData); // Add the item to the inventory
    //        Destroy(gameObject); // Destroy the collectible object
    //    }
    //}
}


