using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image iconImage;
    public Image tintOverlay;
    public TMP_Text countText;
    public TMP_Text itemNameText;
    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            iconImage.sprite = itemData.icon;
            countText.text = itemData.count > 1 ? itemData.count.ToString() : ""; // Set item count
            iconImage.enabled = true;
            tintOverlay.enabled = false;

            if (itemData.icon == null)
            {
                itemNameText.text = itemData.itemName; //show item name
                itemNameText.enabled = true;
            }
            else
            {
                itemNameText.enabled = false;
                iconImage.color = new Color(1.5f, 1.5f, 1.5f, 1.5f);
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
        iconImage.color = new Color(0f, 0f, 0f, 0f);
        countText.text = "";
        itemNameText.enabled = false;
        tintOverlay.enabled = false;
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            tintOverlay.enabled = true;
            tintOverlay.color = new Color(1f, 0f, 1f, 0.15f);
 
        }
        else
        {
            tintOverlay.enabled = false;
        }
    }
}