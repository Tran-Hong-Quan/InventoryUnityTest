using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    public List<Slot> slots;
    [SerializeField] private GameObject inventoryPrefab;

    [HideInInspector] public InventoryItemListData inventoryItemListData=new InventoryItemListData();
    [HideInInspector] public List<InventoryItem> InventoryItemsList = new List<InventoryItem>();

    [SerializeField] private ItemConfig itemConfig;

    public static Inventory instance;
    private void Awake()
    {
        instance = this;

        for (int i = 0; i < slots.Count; i++)
            slots[i].slotId = i;
    }

    void Start()
    {   
        for (int i = 0; i < 6; i++)
        {
            Item book = new Item(ItemType.Book,"book");
            AddItemToInventory(itemConfig.GetItemConfig(book));

            Item gun = new Item(ItemType.Gun, "gun");
            AddItemToInventory(itemConfig.GetItemConfig(gun));

            Item food = new Item(ItemType.Food, "food");
            itemConfig.GetItemConfig(ref food);
            food.amount = 3;
            AddItemToInventory(food);
        }
    }
    /// <summary>
    /// Add a item to a unfilled slot, if success, return true, else return false 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].isFill)
            {
                item.slotId = i;
                SpawnItem(item);
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Add item to a slot, if success, return true, else return false 
    /// </summary>
    /// <param name="item"></param>
    public bool SpawnItem(Item item)
    {
        int i = item.slotId;
        if (!slots[i].isFill)
        {
            slots[i].isFill = true;

            InventoryItem tempInventoryItem = Instantiate(inventoryPrefab.GetComponent<InventoryItem>());
            tempInventoryItem.SetData(inventoryCanvas, item);

            InventoryItemsList.Add(tempInventoryItem);
            inventoryItemListData.inventoryItemListData.Add(item);
            return true;
        }
        return false;
    }
    public void ClearInventory()
    {
        foreach (var item in InventoryItemsList)
            Destroy(item.gameObject);
        InventoryItemsList.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }
    }
    public void CleanInventory()
    {
        List<Item> dataList = inventoryItemListData.inventoryItemListData;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].maxAmount == 1)
                i++;
            for (int j = i + 1; j < dataList.Count; j++)
            {
                if (string.Compare(dataList[i].itemName ,dataList[j].itemName)==0)
                {
                    if(dataList[i].amount+dataList[j].amount<dataList[i].maxAmount)
                        dataList[i].amount+=dataList[j].amount;

                    dataList.RemoveAt(j);
                    j--; 
                }
            }
        }
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }
        foreach (var item in InventoryItemsList)
            Destroy(item.gameObject);
        InventoryItemsList.Clear();

        Item[] tempArray = new Item[inventoryItemListData.inventoryItemListData.Count];
        inventoryItemListData.inventoryItemListData.CopyTo(tempArray);
        inventoryItemListData.inventoryItemListData.Clear();

        for (int i = 0; i < tempArray.Length; i++)
            AddItemToInventory(tempArray[i]);

    }
    public void ChangeSlot(int oldSlot,int newSlot)
    {
        foreach (var item in inventoryItemListData.inventoryItemListData)
            if(item.slotId==oldSlot)
            {
                item.slotId = newSlot;
            }    

    }    
    public bool CanAddItem(int fromSlot, int toSlot)
    {
        Item dropedItem=null, checkingItem=null;
        foreach(var item in inventoryItemListData.inventoryItemListData)
        {
            if(item.slotId==fromSlot)
                dropedItem = item;
            if(item.slotId==toSlot)
                checkingItem= item;
            if (checkingItem != null && dropedItem != null)
                break;
        }

        if (checkingItem.itemType != dropedItem.itemType||
            checkingItem.itemName!=dropedItem.itemName||
            checkingItem.amount==checkingItem.maxAmount||
            checkingItem == null ||
            dropedItem == null)
            return false;

        if(checkingItem.amount+dropedItem.amount<=checkingItem.maxAmount)
        {
            checkingItem.amount+=dropedItem.amount;

            slots[fromSlot].isFill = false;
            slots[fromSlot].isFull = false;

            foreach (var item in InventoryItemsList)
            {
                if (toSlot == item.locateSlotId)
                {
                    item.AmoutText.text = checkingItem.amount.ToString();
                    break;
                }
            }
            foreach (var item in InventoryItemsList)
            {
                if (fromSlot == item.locateSlotId)
                {
                    Destroy(item.gameObject);
                    InventoryItemsList.Remove(item);
                    break;
                }
            }

            inventoryItemListData.inventoryItemListData.Remove(dropedItem);

            return true;
        } 
        else
        {
            dropedItem.amount -= checkingItem.maxAmount-checkingItem.amount;
            checkingItem.amount = checkingItem.maxAmount;

            slots[toSlot].isFull = true;

            foreach (var item in InventoryItemsList)
            {
                if (toSlot == item.locateSlotId)
                {
                    item.AmoutText.text = checkingItem.amount.ToString();
                }
                else if(fromSlot == item.locateSlotId)
                {
                    item.AmoutText.text=dropedItem.amount.ToString();
                }    
            }

            return false;
        }    
  
    }    
    public void ThrowItem(int slotId)
    {
        foreach(var item in inventoryItemListData.inventoryItemListData)
        {
            if(slotId==item.slotId)
            {
                inventoryItemListData.inventoryItemListData.Remove(item);
                break;
            }    
        }    
        foreach(var item in InventoryItemsList)
        {
            if(slotId==item.locateSlotId)
            {
                Destroy(item.gameObject);
                InventoryItemsList.Remove(item);
                break;
            }    
        }    

        slots[slotId].isFull = false;
        slots[slotId].isFill = false;
    }    

    #region Data
    private void SaveData(string data, string fileName)
    {
        string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

        File.WriteAllText(dataPath, data);
    }
    private string LoadData(string fileName)
    {
        string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

        if (File.Exists(dataPath))
            return File.ReadAllText(dataPath);
        else
            return "";
    }
    public void SaveInventoryData()
    {
        Debug.Log(JsonUtility.ToJson(inventoryItemListData));
        SaveData(JsonUtility.ToJson(inventoryItemListData), "InventoryData");
    }
    public void LoadInventoryData()
    {
        string data = LoadData("InventoryData");

        if (data != "")
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].isFull = false;
                slots[i].isFill = false;
            }
            foreach (var item in InventoryItemsList)
                Destroy(item.gameObject);
            InventoryItemsList.Clear();
            inventoryItemListData = new InventoryItemListData();

            InventoryItemListData tempInventoryItemsListData = JsonUtility.FromJson<InventoryItemListData>(data);

            foreach (var item in tempInventoryItemsListData.inventoryItemListData)
            {
                SpawnItem(item);
            }
        }
        else
        {
            inventoryItemListData = new InventoryItemListData();
        }
    }
    #endregion Data

}

[Serializable]
public class InventoryItemListData
{
    public List<Item> inventoryItemListData = new List<Item>();
}

