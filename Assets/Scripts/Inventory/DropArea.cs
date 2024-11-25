using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            draggableItem.transform.SetParent(transform);
            draggableItem.transform.position = transform.position; // Align position to center of the new parent
        }
    }
}
