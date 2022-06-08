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

    [HideInInspector] public InventoryItemData InventoryData;
    [HideInInspector] public InventoryItem[] InventoryItemsArray;
    [SerializeField] private ItemConfig itemConfig;

    public static Inventory instance;
    private void Awake()
    {
        instance = this;

        for (int i = 0; i < slots.Count; i++)
            slots[i].slotId = i;

        InventoryItemsArray = new InventoryItem[slots.Count];
        InventoryData = new InventoryItemData(slots.Count);

    }

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            Item book = new Item(ItemType.Book, "book");
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
            InventoryItemsArray[i] = tempInventoryItem;
            InventoryData.Data[i] = item;
            return true;
        }
        return false;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < InventoryItemsArray.Length; i++)
        {
            if (InventoryItemsArray[i] != null)
            {
                Destroy(InventoryItemsArray[i].gameObject);
                InventoryItemsArray[i] = null;
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }

        InventoryData = new InventoryItemData(slots.Count);
    }
    public void CleanInventory()
    {
        Item[] cleanArray = new Item[InventoryData.Data.Length];
        Item[] sortedArray = new Item[InventoryData.Data.Length];
        int index = 0;

        for (int i = 0; i < InventoryData.Data.Length; i++)
        {
            if (InventoryData.Data[i] == null)
                continue;
            if(InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
            {
                cleanArray[index++] = InventoryData.Data[i];
                continue;
            }    
            for (int j = i + 1; j < InventoryData.Data.Length; j++)
            {
                if (InventoryData.Data[j] == null|| InventoryData.Data[j].amount==InventoryData.Data[j].maxAmount)
                    continue;
                if (string.Compare(InventoryData.Data[i].itemName, InventoryData.Data[j].itemName) == 0)
                {
                    if (InventoryData.Data[i].amount + InventoryData.Data[j].amount <= InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[i].amount += InventoryData.Data[j].amount;
                        InventoryData.Data[j] = null;
                        if( InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
                            break;
                    }    
                    else if(InventoryData.Data[i].amount + InventoryData.Data[j].amount > InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[j].amount-=InventoryData.Data[i].maxAmount- InventoryData.Data[i].amount;
                        InventoryData.Data[i].amount = InventoryData.Data[i].maxAmount;

                        break;
                    }
                }
            }
            cleanArray[index++] = InventoryData.Data[i];
        }

        List<String> nameList=new List<String>();
        for(int i=0; i<cleanArray.Length; i++)
        {
            if (cleanArray[i] == null)
                break;
            string name = cleanArray[i].itemName;
            if(!nameList.Contains(name))
                nameList.Add(name);
        }

        index = 0;
        for(int i=0; i<nameList.Count; i++)
        {
            for (int j = 0; j < cleanArray.Length; j++)
            {
                if (cleanArray[j] == null)
                    break;
                if(nameList[i]==cleanArray[j].itemName)
                {
                    sortedArray[index++] = cleanArray[j];
                }
            }
        }
        


        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull = false;
            slots[i].isFill = false;
        }
        for (int i = 0; i < InventoryItemsArray.Length; i++)
        {
            if(InventoryItemsArray[i] != null)
            {
                Destroy(InventoryItemsArray[i].gameObject);
                InventoryItemsArray[i] = null;
            }          
        }

        for (int i = 0; i < InventoryData.Data.Length; i++)
            InventoryData.Data[i] = null;

        for (int i = 0; i < cleanArray.Length; i++)
            if (sortedArray[i] == null)
                break;
            else
                AddItemToInventory(sortedArray[i]);

    }
    public void ChangeSlot(int oldSlot, int newSlot)
    {
        InventoryData.Data[oldSlot].slotId = newSlot;
        InventoryData.Data[newSlot]=InventoryData.Data[oldSlot];

        InventoryItemsArray[newSlot]=InventoryItemsArray[oldSlot];
        InventoryItemsArray[oldSlot] = null;
    }
    public bool CanAddItem(int fromSlot, int toSlot)
    {
        if (InventoryData.Data[toSlot].itemType != InventoryData.Data[fromSlot].itemType ||
            InventoryData.Data[toSlot].itemName != InventoryData.Data[fromSlot].itemName ||
            InventoryData.Data[toSlot].amount == InventoryData.Data[toSlot].maxAmount ||
            InventoryData.Data[toSlot] == null ||
            InventoryData.Data[fromSlot] == null)
            return false;

        if (InventoryData.Data[toSlot].amount + InventoryData.Data[fromSlot].amount <= InventoryData.Data[fromSlot].maxAmount)
        {
            InventoryData.Data[toSlot].amount += InventoryData.Data[fromSlot].amount;

            slots[fromSlot].isFill = false;
            slots[fromSlot].isFull = false;

            InventoryItemsArray[toSlot].AmoutText.text = InventoryData.Data[toSlot].amount.ToString();

            Destroy(InventoryItemsArray[fromSlot].gameObject);
            InventoryItemsArray[fromSlot] = null;

            InventoryData.Data[fromSlot] = null;

            return true;
        }
        else if(InventoryData.Data[fromSlot].amount< InventoryData.Data[fromSlot].maxAmount)
        {
            InventoryData.Data[fromSlot].amount -= InventoryData.Data[toSlot].maxAmount - InventoryData.Data[toSlot].amount;
            InventoryData.Data[toSlot].amount = InventoryData.Data[toSlot].maxAmount;

            slots[toSlot].isFull = true;

            InventoryItemsArray[toSlot].AmoutText.text = InventoryData.Data[toSlot].amount.ToString();
            InventoryItemsArray[fromSlot].AmoutText.text = InventoryData.Data[fromSlot].amount.ToString();

            return false;
        }
        else
        {
            return false;
        }         
    }
    public void ThrowItem(int slotId)
    {
        InventoryData.Data[slotId]=null;

        Destroy(InventoryItemsArray[slotId].gameObject);
        InventoryItemsArray[slotId] = null;

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
        Debug.Log(JsonUtility.ToJson(InventoryData));
        SaveData(JsonUtility.ToJson(InventoryData), "InventoryData");
    }
    public void LoadInventoryData()
    {
        string data = LoadData("InventoryData");

        if (data != "")
        {
            ClearInventory();

            InventoryItemData tempInventoryItemsListData = JsonUtility.FromJson<InventoryItemData>(data);

            foreach (var item in tempInventoryItemsListData.Data)
            {
                SpawnItem(item);
            }
        }
        else
        {
            InventoryData = new InventoryItemData(slots.Count);
        }
    }
    #endregion Data

}

[Serializable]
public class InventoryItemData
{
    public Item[] Data;

    public InventoryItemData(int length)
    {
        Data = new Item[length];
    }
}


