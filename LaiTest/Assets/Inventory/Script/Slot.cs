using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IDropHandler
{
    [HideInInspector]
    public bool isFill=false;
    [HideInInspector]
    public bool isFull=false;

    [HideInInspector]
    public int slotId;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag!=null&&!isFill)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            isFill = true;

            int lastSlotId = eventData.pointerDrag.GetComponent<DragAndDropItem>().locateSlotId;
            eventData.pointerDrag.GetComponent<DragAndDropItem>().locateSlotId = slotId;
            Inventory.instance.slots[lastSlotId].isFill = false;
            Inventory.instance.slots[lastSlotId].isFull = false;
        }    
        else if(isFull)
        {
            eventData.pointerDrag.GetComponent<DragAndDropItem>().ComeBackToSlot();
        }    
    }
}
