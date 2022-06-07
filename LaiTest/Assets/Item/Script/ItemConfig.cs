using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemConfig",fileName ="Intem Config")]
public class ItemConfig : ScriptableObject
{
    #region Mana

    [Serializable]
    public class ManaFlaskConfig
    {
        public ManaFlask mana;
        public string name;
        public int amount;
        public int maxAmount;
    }
    public List<ManaFlaskConfig> manaRcoveryConfigList;
  
    public ManaFlask GetManaFlaskConfig(string name)
    { 
        for (int i=0;i<manaRcoveryConfigList.Count;i++)
            if(name==manaRcoveryConfigList[i].name)
                return manaRcoveryConfigList[i].mana;
        return null;
    }

    public Item GetManaFlaskItemConfig(string name)
    {
        for (int i = 0; i < manaRcoveryConfigList.Count; i++)
            if (name == manaRcoveryConfigList[i].name)
            {
                Item item = new Item();
                item.amount = manaRcoveryConfigList[i].amount;
                item.maxAmount = manaRcoveryConfigList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(manaRcoveryConfigList[i].mana);
                item.itemName = name;
                return item;
            }
        return null;
    }
    #endregion

    #region HP

    [Serializable]
    public class HealFlaskConfig
    {
        public HealFlask heal;
        public string name;
        public int amount;
        public int maxAmount;
    }
    public List<HealFlaskConfig> healFlaskConfigList; 
    public HealFlask GetHPFlaskConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
                return healFlaskConfigList[i].heal;
        return null;
    }

    public Item GetHPFlaskItemConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
            {
                Item item = new Item();
                item.amount = healFlaskConfigList[i].amount;
                item.maxAmount = healFlaskConfigList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(healFlaskConfigList[i].heal);
                item.itemName = name;
                return item;
            }
        return null;
    }

    #endregion

    #region Food

    public List<FoodConfig> foodConfigList;
    [Serializable]
    public class FoodConfig
    {
        public Food food;
        public string name;
        public int amount;
        public int maxAmount;
    }    
    public Food GetFoodConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
                return foodConfigList[i].food;
        return null;
    }

    public Item GetFoodItemConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
            {
                Item item = new Item();
                item.amount = foodConfigList[i].amount;
                item.maxAmount = foodConfigList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(foodConfigList[i].food);
                item.itemName = name;
                return item;
            }
        return null;
    }

    #endregion

    #region Gun

    public List<GunConfig> gunConfigsList;
    [Serializable]
    public class GunConfig
    {
        public Gun gun;
        public string name;
        public int amount;
        public int maxAmount;
    }
    public Gun GetGunConfig(string name)
    {
        for (int i = 0; i < gunConfigsList.Count; i++)
            if (name == gunConfigsList[i].name)
                return gunConfigsList[i].gun;
        return null;
    }

    public Item GetGunItemConfig(string name)
    {
        for (int i = 0; i < gunConfigsList.Count; i++)
            if (name == gunConfigsList[i].name)
            {
                Item item=new Item();
                item.amount = gunConfigsList[i].amount;
                item.maxAmount = gunConfigsList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(gunConfigsList[i].gun);
                item.itemName = name;
                return item;
            }    
        return null;
    }    

    #endregion

    #region Book

    [Serializable]
    public class BooKConfig
    {
        public Book book;
        public string name;
        public int amount;
        public int maxAmount;
    }
    public List<BooKConfig> bookConfigsList;

    public Book GetBookConfig(string name)
    {
        for (int i = 0; i < bookConfigsList.Count; i++)
            if (name == bookConfigsList[i].name)
                return bookConfigsList[i].book;
        return null;
    }
    public Item GetBookItemConfig(string name)
    {
        for (int i = 0; i < bookConfigsList.Count; i++)
            if (name == bookConfigsList[i].name)
            {
                Item item = new Item();
                item.amount = bookConfigsList[i].amount;
                item.maxAmount = bookConfigsList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(bookConfigsList[i].book);
                item.itemName = name;
                return item;
            }
        return null;
    }


    #endregion

    #region Sword

    [Serializable]
    public class SwordConfig
    {
        public Sword sword;
        public string name;
        public int amount;
        public int maxAmount;
    }
    public List<SwordConfig> swordConfigsList;

    public Sword GetSwordConfig(string name)
    {
        for (int i = 0; i < swordConfigsList.Count; i++)
            if (name == swordConfigsList[i].name)
                return swordConfigsList[i].sword;
        return null;
    }

    public Item GetSwordItemConfig(string name)
    {
        for (int i = 0; i < swordConfigsList.Count; i++)
            if (name == swordConfigsList[i].name)
            {
                Item item = new Item();
                item.amount = swordConfigsList[i].amount;
                item.maxAmount = swordConfigsList[i].maxAmount;
                item.itemData = JsonUtility.ToJson(swordConfigsList[i].sword);
                item.itemName = name;
                return item;
            }
        return null;
    }

    #endregion

    /// <summary>
    /// Get Config data when item have only name
    /// </summary>
    /// <param name="item"></param>
    public Item GetItemConfig(Item item)
    {
        string itemName = item.itemName;
        switch(item.itemType)
        {
            case ItemType.Book:
                item = GetBookItemConfig(item.itemName);
                return item;
            case ItemType.Gun:
                item = GetGunItemConfig(item.itemName);
                return item;
            case ItemType.Food:
                item = GetFoodItemConfig(item.itemName);
                return item;
            case ItemType.LifeFlask:
                item = GetHPFlaskItemConfig(item.itemName);
                return item;
            default:
                return null;
        }    
    }    

    /// <summary>
    /// Get Config data when item have only name
    /// </summary>
    /// <param name="item"></param>
    public void GetItemConfig(ref Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Book:
                item = GetBookItemConfig(item.itemName);
                break;
            case ItemType.Gun:
                item = GetGunItemConfig(item.itemName);
                break;
            case ItemType.Food:
                item= GetFoodItemConfig(item.itemName);
                break;
            case ItemType.LifeFlask:
                item=GetHPFlaskItemConfig(item.itemName);
                break;
            default:
                break;
        }
    }    
}


