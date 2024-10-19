using UnityEngine;
using TMPro; 

public class InventoryUI : MonoBehaviour
{
    public TMP_Text itemCountText; // Reference to the Text component
    private Inventory inventory; // Reference to the Inventory

    void Start()
    {
        inventory = Inventory.Instance; // Get the singleton instance of Inventory
        UpdateItemCount(); // Initialize the text
    }

    void Update()
    {
        UpdateItemCount(); // Continuously update the text (or you can optimize it to only update when items change)
    }

    void UpdateItemCount()
    {
        // Create a string to display the item counts
        string displayText = "Inventory: ";

        foreach (Item item in inventory.items)
        {
          //  displayText += $"{item.name}: {item.count}\n"; // Format the text to show each item's count
           displayText += $"Items: {item.count}";
        }

        itemCountText.text = displayText; // Update the Text component
    }
    
}
