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

    [HideInInspector] public InventoryItemListData inventoryItemListData;
    [HideInInspector] public Dictionary<int,GameObject> InventoryItemsList = new Dictionary<int, GameObject>();

    [SerializeField] private ItemConfig itemConfig;

    public static Inventory instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //for(int i = 0; i < slots.Count; i++)
        //    slots[i].slotId = i;

        //inventoryItemListData = new InventoryItemListData();
        //for (int i = 0; i < 4; i++)
        //{
        //    Item book = new Item();
        //    book.itemType = ItemType.book;
        //    book.itemData = JsonUtility.ToJson(itemConfig.GetBookConfig("book"));
        //    AddItemToInventory(book);

        //    Item gun = new Item();
        //    gun.itemType = ItemType.gun;
        //    gun.itemData = JsonUtility.ToJson(itemConfig.GetGunConfig("gun"));
        //    AddItemToInventory(gun);

        //    Item food = new Item();
        //    food.itemType = ItemType.food;
        //    food.itemData = JsonUtility.ToJson(itemConfig.GetFoodConfig("food"));
        //    AddItemToInventory(food);

        //}

        //SaveInventoryData();

        LoadInventoryData();
        CleanInventory();

    }

    public bool AddItemToInventory(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(!slots[i].isFill)
            {
                slots[i].isFill = true;
                GameObject tempGameObject = Instantiate(inventoryPrefab);
                tempGameObject.GetComponent<DragAndDropItem>().canvas = inventoryCanvas;
                tempGameObject.transform.SetParent(slots[i].transform);
                tempGameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                tempGameObject.GetComponent<DragAndDropItem>().locateSlotId = slots[i].slotId;

                switch(item.itemType)
                {
                    case ItemType.book:
                        Book tempBook=new Book();
                        tempBook=JsonUtility.FromJson<Book>(item.itemData);
                        tempGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/{tempBook.name}");
                        if (tempBook.amount > 1)
                            tempGameObject.GetComponent<InventoryItem>().AmoutText.text = tempBook.amount.ToString();
                        else
                            tempGameObject.GetComponent<InventoryItem>().AmoutText.text = "";
                        break;
                    case ItemType.gun:
                        Gun tempGun = new Gun();
                        tempGun = JsonUtility.FromJson<Gun>(item.itemData);
                        tempGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/{tempGun.name}");
                        tempGameObject.GetComponent<InventoryItem>().AmoutText.text = "";
                        break;
                    case ItemType.food:
                        Food tempFood=new Food();
                        tempFood = JsonUtility.FromJson<Food>(item.itemData);
                        tempGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/{tempFood.name}");
                        if (tempFood.amount > 1)
                            tempGameObject.GetComponent<InventoryItem>().AmoutText.text = tempFood.amount.ToString();
                        else
                            tempGameObject.GetComponent<InventoryItem>().AmoutText.text = "";
                        break;
                }    

                InventoryItemsList.Add(i, tempGameObject);

                InventoryItemData itemData = new InventoryItemData();
                itemData.slotId = i;
                itemData.item=item;

                inventoryItemListData.inventoryItemListData.Add(itemData);

                return true;
            }    
        }    

        return false;
    }

    public void ResetInventory()
    {
        foreach (var item in InventoryItemsList)
            Destroy(item.Value.gameObject);
        InventoryItemsList.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].isFull=false;
            slots[i].isFill = false;
        }    

        foreach (var itemInventoryData in inventoryItemListData.inventoryItemListData)
        {
            AddItemToInventory(itemInventoryData.item);
        }
    }

    private void CleanInventory()
    {
        List<InventoryItemData> dataList = inventoryItemListData.inventoryItemListData;
        for (int i = 0; i < dataList.Count; i++)
        {
            for(int j=i+1;j<dataList.Count;j++)
            {
                if(dataList[i].item.itemType==dataList[j].item.itemType)
                {
                    switch(dataList[i].item.itemType)
                    {
                        case ItemType.book:
                            Book tempBook1 = JsonUtility.FromJson<Book>(dataList[i].item.itemData);
                            Book tempBook2 = JsonUtility.FromJson<Book>(dataList[j].item.itemData);

                            if(tempBook1.name==tempBook2.name)
                            {
                                tempBook1.amount+=tempBook2.amount;
                                
                            }
                            dataList[i].item.itemData=JsonUtility.ToJson(tempBook1);
                            dataList.RemoveAt(j);
                            break;

                        case ItemType.food:
                            Food tempFood1 = JsonUtility.FromJson<Food>(dataList[i].item.itemData);
                            Food tempFood2 = JsonUtility.FromJson<Food>(dataList[j].item.itemData);

                            if (tempFood1.name == tempFood2.name)
                            {
                                tempFood1.amount += tempFood2.amount;

                            }
                            dataList[i].item.itemData = JsonUtility.ToJson(tempFood1);
                            dataList.RemoveAt(j);
                            break;
                    }
                    //j--;
                }    
            }    
        }   
        
        ResetInventory();
    }    

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
    
    private void SaveInventoryData()
    {
        Debug.Log(JsonUtility.ToJson(inventoryItemListData));
        SaveData(JsonUtility.ToJson(inventoryItemListData), "InventoryData");
    }

    private void LoadInventoryData()
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
                Destroy(item.Value.gameObject);
            InventoryItemsList.Clear();
            inventoryItemListData = new InventoryItemListData();

            InventoryItemListData tempInventoryItemsListData = JsonUtility.FromJson<InventoryItemListData>(data);

            foreach (var item in tempInventoryItemsListData.inventoryItemListData)
            {
                AddItemToInventory(item.item);
            }
        }
        else
        {
            inventoryItemListData = new InventoryItemListData();
        }
    }

}

[Serializable]
public class InventoryItemListData
{
    public List<InventoryItemData> inventoryItemListData=new List<InventoryItemData>();
}

[Serializable]
public class InventoryItemData
{
    public Item item;
    public int slotId;
}
