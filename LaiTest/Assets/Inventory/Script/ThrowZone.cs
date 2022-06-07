using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ThrowZone : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            Inventory.instance.ThrowItem(eventData.pointerDrag.transform.GetComponent<InventoryItem>().locateSlotId);
        }    
    }
}
