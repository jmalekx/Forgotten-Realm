
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private ItemData item;
    public int index;

    public Image iconImage;
    public Image tintOverlay;
    public TMP_Text countText;
    public TMP_Text itemNameText;
    private CraftingManager craftingManager;
    //public InventoryManager inventoryManager;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    //private GameObject draggedItemImage;

    private GameObject itemVisual;
    private Canvas canvas;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // Ensure RectTransform reference
        canvasGroup = GetComponent<CanvasGroup>();
        craftingManager = FindObjectOfType<CraftingManager>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Awake()
    {
        craftingManager = FindObjectOfType<CraftingManager>();
        if (craftingManager == null)
        {
            Debug.LogError("CraftingManager not found in the scene.");
        }
    }

    //--------------------------------------------------------------------------------------------------------------

    public void UpdateSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            item = itemData;
            iconImage.sprite = itemData.icon;
            countText.text = itemData.count > 1 ? itemData.count.ToString() : ""; // Set item count
            iconImage.enabled = true;
            tintOverlay.enabled = false;

            if (itemData.icon == null)
            {
                itemNameText.text = itemData.itemName; // Show item name
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



    //--------------------------------------------------------------------------------------------------------------

    // Clear the slot when no item is assigned
    public void ClearSlot()
    {

        item = null;
        iconImage.sprite = null;
        iconImage.color = new Color(0f, 0f, 0f, 0f);
        countText.text = "";
        itemNameText.enabled = false;
        tintOverlay.enabled = false;
    }


    //--------------------------------------------------------------------------------------------------------------


    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            tintOverlay.enabled = true;
            tintOverlay.color = new Color(0.6f, 0f, 0.9f, 0.5f);
        }
        else
        {
            tintOverlay.enabled = false;
        }
    }

    private bool isSlotFilled()
    {
        return !isSlotEmpty();
    }
    
    public bool isSlotEmpty()
    {
        return item == null;
    }
    //--------------------------------------------------------------------------------------------------------------

    // Start dragging the item
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSlotFilled())
        {
            // Store the original position
            originalPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;

            // Create a visual copy of the item to follow the mouse
            itemVisual = Instantiate(gameObject, canvas.transform);
            itemVisual.GetComponent<CanvasGroup>().alpha = 0.6f;  // Make it semi-transparent
            itemVisual.GetComponent<RectTransform>().position = rectTransform.position;  // Start at the original position

            // Create a new GameObject to represent the dragged item image

            //draggedItemImage = new GameObject("DraggedItemImage");
            //draggedItemImage.transform.SetParent(canvas.transform, false);
            //Image image = draggedItemImage.AddComponent<Image>();
            //image.sprite = iconImage.sprite;

            // Match the size of the dragged item image to the original icon size

            //RectTransform draggedRectTransform = draggedItemImage.GetComponent<RectTransform>();
            //draggedRectTransform.sizeDelta = rectTransform.sizeDelta;

            //CanvasGroup draggedCanvasGroup = draggedItemImage.AddComponent<CanvasGroup>();
            //draggedCanvasGroup.blocksRaycasts = false;

            // Ensure the image is fully visible
            //draggedCanvasGroup.alpha = 1f;

            // Update the slot with the reduced item count
            if (item.count > 1)
            {
                item.count -= 1;
                UpdateSlot(item);
            }
            else
            {
                // Clear the slot if only one item is picked up
                //ClearSlot();
            }

            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            //draggedItemImage.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
    }

    //private void 


    //--------------------------------------------------------------------------------------------------------------

    // While dragging the item
    public void OnDrag(PointerEventData eventData)
    {
        //if (draggedItemImage != null)
        //{
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        //    draggedItemImage.GetComponent<RectTransform>().anchoredPosition = localPoint;
        //}


        // Update the position of the item visual to follow the mouse
        itemVisual.GetComponent<RectTransform>().position = eventData.position;

       

    }


    //--------------------------------------------------------------------------------------------------------------

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (draggedItemImage != null)
        //{
        //    Destroy(draggedItemImage);
        //}

        
        //Destroy(itemVisual);
       



        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;




        // If the item wasn't dropped over a valid slot, reset its position
        if (eventData.pointerEnter == null || !IsValidDrop(eventData.pointerEnter))
        {
            rectTransform.anchoredPosition = originalPosition;
            if (item != null && item.count > 0)
            {
                item.count += 1;
                UpdateSlot(item);
            }
            else
            {
                item = null;
                ClearSlot();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------------------
    public void OnDrop(PointerEventData eventData)
    {
        // 1. Destroy dragged clone
        // 2. Update slot item
        // 3. Notify crafting manager for the item change (to validate and create craft item)
            // Validate:
            // 3.1 if valid create craft item
            // 3.2 if not valid: empty the crafting item slot

        
        
        Destroy(itemVisual);
       


        // Get the source slot from the event data
        ItemSlot sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();

       

        if (sourceSlot != null && sourceSlot.isSlotFilled())
        {
            
            UpdateSlot(sourceSlot.item);
            //craftingManager.OnItemDropped(sourceSlot.item, this);

            //TODO: createCraftItem();


            

            // Check if both crafting slots are occupied
            ItemSlot craftingSlot1 = craftingManager.GetCraftingSlot1();
            ItemSlot craftingSlot2 = craftingManager.GetCraftingSlot2();

            
            if (craftingSlot1 == null || craftingSlot2 == null)
            {
                Debug.LogError("One or both crafting slots are not assigned!");
                return;
            }

            
            if (craftingSlot1.isSlotEmpty() || craftingSlot2.isSlotEmpty())
            {
                Debug.Log("Both crafting slots must be occupied before crafting.");
                return;
            }

           
            bool validCombination = craftingManager.CheckForValidCraftingSlotsCombination(craftingSlot1.item, craftingSlot2.item);

            if (validCombination)
            {
                
                craftingManager.CraftItem(craftingSlot1.item, craftingSlot2.item);

                craftingSlot1.ClearSlot();
                craftingSlot2.ClearSlot();


                craftingManager.ClearCraftingSlots();
                //craftingManager.OnDaggerDroppedIntoInventory();
                //inventoryManager.AddItemToInventory(craftingManager.craftedDagger.GetComponent<ItemData>());



            }
            else
            {
                // Log that the combination is not valid and return the items
                Debug.Log("Invalid combination. Returning items to the inventory.");
                sourceSlot.UpdateSlot(sourceSlot.item); // Return the item back to its original slot
            }
        }
    }


    //--------------------------------------------------------------------------------------------------------------


    private bool IsValidDrop(GameObject dropTarget)
    {
        // Check if the drop target is a valid slot (e.g., CraftingSlot)
        return dropTarget != null && dropTarget.GetComponent<ItemSlot>() != null;
    }

}

