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
        if(eventData.pointerDrag != null)
        {
            int lastSlotId = eventData.pointerDrag.GetComponent<InventoryItem>().locateSlotId;
            if (!isFill)
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                isFill = true;
                eventData.pointerDrag.GetComponent<InventoryItem>().locateSlotId = slotId;
                //Inventory.instance.ChangeSlot(lastSlotId, slotId);
                Inventory.instance.slots[lastSlotId].isFill = false;
                Inventory.instance.slots[lastSlotId].isFull = false;
            }
            // else if (!isFull)
            // {
            //     if(!Inventory.instance.CanAddItem(lastSlotId,slotId))
            //         eventData.pointerDrag.GetComponent<InventoryItem>().ComeBackToSlot();
            // }
            // else
            // {
            //     eventData.pointerDrag.GetComponent<InventoryItem>().ComeBackToSlot();
            // }    
        }    
        
    }
}
