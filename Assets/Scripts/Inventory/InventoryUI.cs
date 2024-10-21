//using UnityEngine;
//using TMPro;

//public class InventoryUI : MonoBehaviour
//{
//    public TMP_Text itemCountText; // Reference to the Text component
//    private Inventory inventory; // Reference to the Inventory

////--------------------------------------------------------------------------------------------------------------------
//    void Start()
//    {
//        inventory = Inventory.Instance; // get the singleton instance of Inventory
//        itemCountText = GetComponent<TMP_Text>();
//        UpdateItemCount(); // initialize the text
//    }

//    void Update()
//    {
//        UpdateItemCount(); //continuously update the text 
//    }

////--------------------------------------------------------------------------------------------------------------------
//    void UpdateItemCount()
//    {
//        // Create a string to display the item counts
//        string displayText = "Inventory:\n";

//        foreach (ItemData item in inventory.items)
//        {
//            displayText += $"{item.itemName}: {item.count}\n"; 
//        }

//        itemCountText.text = displayText; // Update the Text component
//    }

//}
