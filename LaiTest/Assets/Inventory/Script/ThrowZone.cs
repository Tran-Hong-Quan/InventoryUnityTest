using UnityEngine;
using UnityEngine.EventSystems;
public class ThrowZone : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<InventoryItem>() != null)
        {
            Inventory.instance.SetNullDraggedItem();
            Destroy(eventData.pointerDrag.gameObject);
        }    
    }
}
