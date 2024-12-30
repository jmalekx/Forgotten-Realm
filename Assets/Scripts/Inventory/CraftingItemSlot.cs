using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//inherit from itemSlot

public class CraftingItemSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public CraftingManager craftingManager;    

    public override void UpdateSlot(ItemData itemData)
    {
        base.UpdateSlot(itemData);
       
    }   

    //public void OnBeginDrag(PointerEventData eventData)
    //{
        
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
        
    //}

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        // Get the source slot from the event data
        ItemSlot sourceSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();

        if (sourceSlot != null && sourceSlot.isSlotFilled())
        {
            
            //notify the crafting manager about the item change
            craftingManager.HandleItemDrop(sourceSlot.item, this);
            sourceSlot.ClearSlot();
        }

    }

    private bool IsValidDrop(GameObject dropTarget)
    {
        // Check if the drop target is a valid slot (e.g., CraftingSlot)
        return dropTarget != null && dropTarget.GetComponent<ItemSlot>() != null;
    }

    //public void OnEndDrag(PointerEventData eventData)
    //{

    //}
}
