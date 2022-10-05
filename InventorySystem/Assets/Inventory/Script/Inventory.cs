﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    public List<Slot> slots;
    [SerializeField] private InventoryItem inventoryPrefab;

    [HideInInspector] public InventoryItemData InventoryData;
    [HideInInspector] public InventoryItem[] InventoryItemsArray;
    public ItemConfig itemConfig;

    [HideInInspector] public InventoryItem draggedItem;
    [HideInInspector] public Item draggedItemData;
    [HideInInspector] public int lastSlotID;

    public UIButton takeOneItemButton;
    public UIButton takeHalfItemButton;

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
        for (int i = 0; i < 4; i++)
        {
            Item book = new Item(ItemType.Book, "book");
            book = itemConfig.GetItemConfig(book);
            book.amount = 5;
            AddItemToInventory(book);

            Item gun = new Item(ItemType.Gun, "gun");
            gun = itemConfig.GetItemConfig(gun);
            AddItemToInventory(gun);

            Item food = new Item(ItemType.Food, "food");
            itemConfig.GetItemConfig(ref food);
            food.amount = 3;
            AddItemToInventory(food);

            Item sword = new Item(ItemType.Sword, "sword");
            itemConfig.GetItemConfig(ref sword);
            AddItemToInventory(sword);

            Item mana = new Item(ItemType.ManaFlask, "mana");
            itemConfig.GetItemConfig(ref mana);
            mana.amount = 3;
            AddItemToInventory(mana);

            Item hp = new Item(ItemType.LifeFlask, "hp");
            hp=itemConfig.GetItemConfig(hp);
            hp.amount = 3;
            AddItemToInventory(hp);
        }

    }
    /// <summary>
    /// Add a item to a slot, if success, return true, else return false 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(Item item)
    {
        if (item.slotId != -1)
        {
            if (!slots[item.slotId].isFill)
            {
                slots[item.slotId].isFill = true;
                if (item.amount >= item.maxAmount) slots[item.slotId].isFull = true;

                if (item.amount > item.maxAmount)
                {
                    Item offsetItem = new Item(item.itemType, item.itemData, item.itemName, -1, item.maxAmount - item.amount,item.maxAmount);
                    item.amount = item.maxAmount;
                    AddItemToInventory(offsetItem);
                }

                InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
                tempInventoryItem.SetData(inventoryCanvas, item);

                InventoryItemsArray[item.slotId] = tempInventoryItem;
                InventoryData.Data[item.slotId] = item;
                return true;
            }
            else if (item.itemName == InventoryData.Data[item.slotId].itemName && item.itemType == InventoryData.Data[item.slotId].itemType)
            {
                if (item.amount + InventoryData.Data[item.slotId].amount <= item.maxAmount)
                {
                    InventoryData.Data[item.slotId].amount += item.amount;
                    InventoryItemsArray[item.slotId].amoutText.text = InventoryData.Data[item.slotId].amount.ToString();

                    if (InventoryData.Data[item.slotId].amount == InventoryData.Data[item.slotId].maxAmount) slots[item.slotId].isFull = true;
                }
                else
                {
                    item.amount -= InventoryData.Data[item.slotId].maxAmount - InventoryData.Data[item.slotId].amount;
                    InventoryData.Data[item.slotId].amount = InventoryData.Data[item.slotId].maxAmount;
                    InventoryItemsArray[item.slotId].amoutText.text = InventoryData.Data[item.slotId].amount.ToString();
                    slots[item.slotId].isFull = true;

                    item.slotId = -1;
                    AddItemToInventory(item);
                }
            }
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (!slots[i].isFill)
                {
                    item.slotId = i;
                    slots[i].isFill = true;
                    if (item.amount >= item.maxAmount) slots[item.slotId].isFull = true;

                    if (item.amount > item.maxAmount)
                    {
                        Item offsetItem = new Item(item.itemType, item.itemData, item.itemName, -1, item.amount - item.maxAmount,item.maxAmount);
                        AddItemToInventory(offsetItem);
                        item.amount = item.maxAmount;
                    }
                    InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
                    tempInventoryItem.SetData(inventoryCanvas, item);

                    InventoryItemsArray[i] = tempInventoryItem;
                    InventoryData.Data[i] = item;
                    return true;


                }
                else if (!slots[i].isFull)
                {
                    if (item.itemName == InventoryData.Data[i].itemName)
                        if (item.amount + InventoryData.Data[i].amount <= item.maxAmount)
                        {
                            InventoryData.Data[i].amount += item.amount;
                            InventoryItemsArray[i].amoutText.text = InventoryData.Data[i].amount.ToString();

                            if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount) slots[i].isFull = true;
                            return true;
                        }
                        else
                        {
                            item.amount -= InventoryData.Data[i].maxAmount - InventoryData.Data[i].amount;
                            InventoryData.Data[i].amount = InventoryData.Data[i].maxAmount;
                            InventoryItemsArray[i].amoutText.text = InventoryData.Data[i].amount.ToString();
                            slots[i].isFull = true;

                            AddItemToInventory(item);
                            return true;
                        }
                }
            }
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

        if (draggedItem != null)
            Destroy(draggedItem.gameObject);
        SetNullDraggedItem();

        InventoryData = new InventoryItemData(slots.Count);
    }
    public void CleanInventory()
    {
        Item[] cleanArray = new Item[InventoryData.Data.Length];
        Item[] sortedArray = new Item[InventoryData.Data.Length];
        int index = 0;

        for (int i = 0; i < InventoryData.Data.Length; i++)
        {
            if (InventoryData.Data[i] == null || InventoryData.Data[i].itemName == "")
                continue;
            if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
            {
                cleanArray[index++] = InventoryData.Data[i];
                continue;
            }
            for (int j = i + 1; j < InventoryData.Data.Length; j++)
            {
                if (InventoryData.Data[j] == null || InventoryData.Data[j].amount == InventoryData.Data[j].maxAmount)
                    continue;
                if (string.Compare(InventoryData.Data[i].itemName, InventoryData.Data[j].itemName) == 0)
                {
                    if (InventoryData.Data[i].amount + InventoryData.Data[j].amount <= InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[i].amount += InventoryData.Data[j].amount;
                        InventoryData.Data[j] = null;
                        if (InventoryData.Data[i].amount == InventoryData.Data[i].maxAmount)
                            break;
                    }
                    else if (InventoryData.Data[i].amount + InventoryData.Data[j].amount > InventoryData.Data[i].maxAmount)
                    {
                        InventoryData.Data[j].amount -= InventoryData.Data[i].maxAmount - InventoryData.Data[i].amount;
                        InventoryData.Data[i].amount = InventoryData.Data[i].maxAmount;

                        break;
                    }
                }
            }
            cleanArray[index++] = InventoryData.Data[i];
        }

        List<String> nameList = new List<String>();
        for (int i = 0; i < cleanArray.Length; i++)
        {
            if (cleanArray[i] == null || cleanArray[i].itemName == "")
                continue;
            string name = cleanArray[i].itemName;
            if (!nameList.Contains(name))
                nameList.Add(name);
        }

        index = 0;
        for (int i = 0; i < nameList.Count; i++)
        {
            if (nameList[i] == "")
                continue;
            for (int j = 0; j < cleanArray.Length; j++)
            {
                if (cleanArray[j] == null)
                    continue;
                if (nameList[i] == cleanArray[j].itemName)
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
            if (InventoryItemsArray[i] != null)
            {
                Destroy(InventoryItemsArray[i].gameObject);
                InventoryItemsArray[i] = null;
            }
        }

        for (int i = 0; i < InventoryData.Data.Length; i++)
            InventoryData.Data[i] = null;

        for (int i = 0; i < sortedArray.Length; i++)
            if (sortedArray[i] == null || sortedArray[i].itemName == "")
                continue;
            else
            {
                sortedArray[i].slotId = -1;
                AddItemToInventory(sortedArray[i]);
            }

    }

    public void SetDraggedItem(InventoryItem inventoryItem, int draggedAmount)
    {
        draggedItem = inventoryItem;
        lastSlotID = draggedItem.locateSlotId;

        ItemType type = InventoryData.Data[lastSlotID].itemType;
        string name = InventoryData.Data[lastSlotID].itemName;
        draggedItemData = new Item(type, name);
        itemConfig.GetItemConfig(ref draggedItemData);
        draggedItemData.amount = draggedAmount;

        InventoryData.Data[lastSlotID].amount -= draggedAmount;
        InventoryItem tempInventoryItem = Instantiate(inventoryPrefab);
        tempInventoryItem.SetData(inventoryCanvas, InventoryData.Data[lastSlotID]);
        InventoryItemsArray[lastSlotID] = tempInventoryItem;

        slots[lastSlotID].isFull = false;

        if (InventoryData.Data[lastSlotID].amount == 1) InventoryItemsArray[lastSlotID].amoutText.text = ""; else InventoryItemsArray[lastSlotID].amoutText.text = InventoryData.Data[lastSlotID].amount.ToString();

        draggedItem.locateSlotId = -1;
        draggedItemData.slotId = -1;
    }
    public void SetDraggedItem(InventoryItem inventoryItem)
    {
        draggedItem = inventoryItem;
        draggedItemData = InventoryData.Data[draggedItem.locateSlotId];
        lastSlotID = draggedItem.locateSlotId;

        InventoryData.Data[lastSlotID] = null;
        InventoryItemsArray[lastSlotID] = null;

        slots[lastSlotID].isFill = false;
        slots[lastSlotID].isFull = false;

        draggedItem.locateSlotId = -1;
        draggedItemData.slotId = -1;
    }
    public void SetDraggedItemOutsideInventory(InventoryItem draggedItem, Item itemData) //This fuction called if you want drag a item outside the inventory
    {
        this.draggedItem = draggedItem;
        draggedItemData = itemData;
        lastSlotID = -1;
    }
    public void DropItem(Slot slot)
    {
        if (!slot.isFill)
        {
            Debug.Log("Drop to unfilled slot");
            slot.DropItem(draggedItem);
            if (draggedItemData.amount == draggedItemData.maxAmount) slot.isFull = true;
            slot.isFill = true;

            InventoryData.Data[slot.slotId] = draggedItemData;
            InventoryData.Data[slot.slotId].slotId = slot.slotId;
            InventoryItemsArray[slot.slotId] = draggedItem;

        }
        else if (!slot.isFull && draggedItemData.itemName == InventoryData.Data[slot.slotId].itemName)
        {
            if (InventoryData.Data[slot.slotId].amount != InventoryData.Data[slot.slotId].maxAmount)
            {
                if (draggedItemData.amount + InventoryData.Data[slot.slotId].amount <= InventoryData.Data[slot.slotId].maxAmount)
                {
                    //Debug.Log("Drop to unfull slot and sum of item amout is less or equal than max amount");
                    InventoryData.Data[slot.slotId].amount += draggedItemData.amount;
                    InventoryItemsArray[slot.slotId].amoutText.text = InventoryData.Data[slot.slotId].amount.ToString();

                    Destroy(draggedItem.gameObject);
                    if (InventoryData.Data[slot.slotId].amount == InventoryData.Data[slot.slotId].maxAmount) slot.isFull = true;

                }
                else
                {
                    draggedItemData.amount -= InventoryData.Data[slot.slotId].maxAmount - InventoryData.Data[slot.slotId].amount;
                    InventoryData.Data[slot.slotId].amount = InventoryData.Data[slot.slotId].maxAmount;
                    slot.isFull = true;
                    InventoryItemsArray[slot.slotId].amoutText.text = InventoryData.Data[slot.slotId].amount.ToString();

                    if (lastSlotID == -1)
                    {
                        AddItemToInventory(draggedItemData);
                    }
                    else if (!slots[lastSlotID].isFill)
                    {
                        //Debug.Log("Drop to unfull slot and sum of item amount is greater max amount and last slot is NOT FILLED");
                        slots[lastSlotID].DropItem(draggedItem);
                        slot.isFull = true;
                        draggedItem.amoutText.text = draggedItemData.amount.ToString();
                        InventoryItemsArray[lastSlotID] = draggedItem;
                        InventoryData.Data[lastSlotID] = draggedItemData;
                        InventoryData.Data[lastSlotID].slotId = lastSlotID;
                        if (InventoryData.Data[lastSlotID].amount <= 1) InventoryItemsArray[lastSlotID].amoutText.text = "";
                    }
                    else if (!slots[lastSlotID].isFull)
                    {
                        //Debug.Log("Drop to unfull slot and sum of item amount is greater max amount and last slot is FILLED and UNFULL");
                        InventoryData.Data[lastSlotID].amount += draggedItemData.amount;
                        InventoryItemsArray[lastSlotID].amoutText.text = InventoryData.Data[lastSlotID].amount.ToString();
                        if (InventoryData.Data[lastSlotID].amount >= InventoryData.Data[lastSlotID].maxAmount) slots[lastSlotID].isFull = true;
                        Destroy(draggedItem.gameObject);
                    }

                }
            }

        }
        else
        {
            if (lastSlotID != -1)
            {
                if (!slots[lastSlotID].isFill)
                {
                    //Debug.Log("Drop to an unavailable slot and come back to EMPTY slot");
                    slots[lastSlotID].DropItem(draggedItem);
                    InventoryData.Data[lastSlotID] = draggedItemData;
                    InventoryItemsArray[lastSlotID] = draggedItem;
                }
                else if (!slots[lastSlotID].isFull)
                {
                    //Debug.Log("Drop to an unavailable slot and come back to LAST slot");
                    InventoryData.Data[lastSlotID].amount += draggedItemData.amount;
                    InventoryItemsArray[lastSlotID].amoutText.text = InventoryData.Data[lastSlotID].amount.ToString();
                    if (InventoryData.Data[lastSlotID].amount >= InventoryData.Data[lastSlotID].maxAmount) slots[lastSlotID].isFull = true;
                    Destroy(draggedItem.gameObject);
                }

            }

        }

        SetNullDraggedItem();
    }

    public void CombackToLastSlot()
    {
        DropItem(slots[lastSlotID]);
    }

    public void SetNullDraggedItem()
    {
        draggedItem = null;
        draggedItemData = null;
        lastSlotID = -1;
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
        //Debug.Log(JsonUtility.ToJson(InventoryData));
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
                if (item == null || item.itemName == "")
                    continue;
                AddItemToInventory(item);
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


