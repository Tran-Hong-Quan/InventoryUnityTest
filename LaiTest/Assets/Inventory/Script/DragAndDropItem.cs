using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropItem : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Canvas canvas;
    public int locateSlotId;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (transform.parent == canvas.transform)
            ComeBackToSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void ComeBackToSlot()
    {
        Slot lastSlot = Inventory.instance.slots[locateSlotId];
        transform.SetParent(lastSlot.transform);
        rectTransform.anchoredPosition3D = Vector3.zero;
    }    
         
}
