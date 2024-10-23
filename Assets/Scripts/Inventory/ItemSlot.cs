using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text countText;
    public TMP_Text itemNameText;
    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            iconImage.sprite = itemData.icon;
            countText.text = itemData.count > 1 ? itemData.count.ToString() : ""; // Set item count
            iconImage.enabled = true;

            if (itemData.icon == null)
            {
                itemNameText.text = itemData.itemName; //show item name
                itemNameText.enabled = true;
            }
            else
            {
                itemNameText.enabled = false; 
            }
        }
        else
        {
            ClearSlot(); // Clear the slot if no item is present
        }
    }

    // Clear the slot when no item is assigned
    public void ClearSlot()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
        countText.text = "";
        itemNameText.enabled = false;
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            iconImage.color = new Color(1f, 0f, 1f, 1f); // Purple
            countText.color = new Color(1f, 0f, 1f, 1f); 
        }
        else
        {
            iconImage.color = iconImage.sprite != null ? Color.white : new Color(0f, 0f, 0f, 0f); // Reset or transparent if empty
            countText.color = Color.white; //
        }
    }
}