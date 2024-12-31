using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemSlot : MonoBehaviour, IDropHandler
{

    public CraftingManager craftingManager;
    public Image iconImage;
    public Image tintOverlay;

    public ItemData item;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private GameObject draggedCloneVisual;
    private Canvas canvas;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // Ensure RectTransform reference
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public ItemData GetItem()
    {
        return item; 
    }

    //--------------------------------------------------------------------------------------------------------------
    private void UpdateSlotIfNeeded(PointerEventData eventData)
    {
        // Get the source slot from the event data
       
        ItemSlot sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();

        if (sourceSlot != null && sourceSlot.isSlotFilled())
        {
            UpdateSlot(sourceSlot.GetItem());
        }
    }

    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            item = itemData;
            iconImage.sprite = itemData.icon;
            iconImage.enabled = true;
            tintOverlay.enabled = false;
            iconImage.color = new Color(1.5f, 1.5f, 1.5f, 1.5f);

        }
    }
    //--------------------------------------------------------------------------------------------------------------

    // Clear the slot when no item is assigned
    public void ClearSlot()
    {
        item = null;
        iconImage.sprite = null;
        iconImage.color = new Color(0f, 0f, 0f, 0f);
        tintOverlay.enabled = false;
    }

    //--------------------------------------------------------------------------------------------------------------
    public void SetSelected(bool isSelected)
    {
        tintOverlay.enabled = true;
        tintOverlay.color = new Color(0.6f, 0f, 0.9f, 0.5f);
    }

    public bool IsSlotFilled()
    {
        return !isSlotEmpty();
    }

    public bool isSlotEmpty()
    {
        return item == null;
    }
  
    //--------------------------------------------------------------------------------------------------------------
    public void OnDrop(PointerEventData eventData)

    {
       
        Destroy(draggedCloneVisual);    

        Debug.Log("CraftingItemSlot::OnDrop");
        ItemData oldItem = item; 
        ClearSlot();
        

        // 1. Update slot item
        UpdateSlotIfNeeded(eventData);

        ItemData newItem = item;
        // 2. Notify crafting manager for the item change (to validate and create craft item)
        craftingManager.HandleSlotItemChange(oldItem, newItem); 
    }
}
