using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text countText;
    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            itemNameText.text = itemData.itemName;
            countText.text = itemData.count > 1 ? itemData.count.ToString() : ""; // Set item count
        }
        else
        {
            ClearSlot(); // Clear the slot if no item is present
        }
    }

    // Clear the slot when no item is assigned
    public void ClearSlot()
    {
        itemNameText.text = "";
        countText.text = "";
    }
}