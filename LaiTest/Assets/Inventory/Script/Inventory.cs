using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    public List<Slot> slots;
    [SerializeField] private GameObject inventoryPrefab;

    [HideInInspector] public List<GameObject> InventoryItems=new List<GameObject>();

    public static Inventory instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for(int i = 0; i < slots.Count; i++)
            slots[i].slotId = i;

        for(int i = 0; i < 10; i++)
        {
            GameObject tempGameObject = Instantiate(inventoryPrefab);
            tempGameObject.GetComponent<DragAndDropItem>().canvas = inventoryCanvas;
            for(int j = 0; j < slots.Count; j++)
            {
                if(!slots[j].isFill)
                {
                    tempGameObject.transform.SetParent(slots[j].transform);
                    tempGameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                    tempGameObject.GetComponent<DragAndDropItem>().locateSlotId = slots[j].slotId;
                    slots[j].isFill = true;
                    break;
                }    
            }    
        }    
    }


    void Update()
    {
        
    }
}
