using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private ItemData item;

    private Image iconImage;
    private Image tintOverlay;
    private TMP_Text countText;
    private TMP_Text itemNameText;
    private CraftingManager craftingManager;


    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private GameObject itemVisual;
    private Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // Ensure RectTransform reference
        canvasGroup = GetComponent<CanvasGroup>();
        craftingManager = FindObjectOfType<CraftingManager>();
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
