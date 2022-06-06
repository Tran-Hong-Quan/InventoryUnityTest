using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemConfig",fileName ="Intem Config")]
public class ItemConfig : ScriptableObject
{
    #region Mana

    public List<ManaFlask> manaRcoveryConfigList;
  
    public ManaFlask GetManaFlaskConfig(string name)
    { 
        for (int i=0;i<manaRcoveryConfigList.Count;i++)
            if(name==manaRcoveryConfigList[i].name)
                return manaRcoveryConfigList[i];
        return null;
    }
    #endregion

    #region HP

    public List<HealFlask> healFlaskConfigList; 
    public HealFlask GetHPFlaskConfig(string name)
    {
        for (int i = 0; i < healFlaskConfigList.Count; i++)
            if (name == healFlaskConfigList[i].name)
                return healFlaskConfigList[i];
        return null;
    }

    #endregion

    #region Food

    public List<Food> foodConfigList;
    public Food GetFoodConfig(string name)
    {
        for (int i = 0; i < foodConfigList.Count; i++)
            if (name == foodConfigList[i].name)
                return foodConfigList[i];
        return null;
    }

    #endregion

    #region Gun

    public List<Gun> gunConfigsList;

    public Gun GetGunConfig(string name)
    {
        for (int i = 0; i < gunConfigsList.Count; i++)
            if (name == gunConfigsList[i].name)
                return gunConfigsList[i];
        return null;
    }

    #endregion

    #region Book

    public List<Book> bookConfigsList;

    public Book GetBookConfig(string name)
    {
        for (int i = 0; i < bookConfigsList.Count; i++)
            if (name == bookConfigsList[i].name)
                return bookConfigsList[i];
        return null;
    }

    #endregion

    #region Sword

    public List<Sword> swordConfigsList;

    public Sword GetSwordConfig(string name)
    {
        for (int i = 0; i < swordConfigsList.Count; i++)
            if (name == swordConfigsList[i].name)
                return swordConfigsList[i];
        return null;
    }

    #endregion

    public Item GetItemConfig(Item item)
    {
        switch(item.itemType)
        {
            case ItemType.book:
                Book tempBook=new Book();
                tempBook=JsonUtility.FromJson<Book>(item.itemData);
                tempBook=GetBookConfig(tempBook.name);
                item.itemData=JsonUtility.ToJson(tempBook);
                return item;
            case ItemType.gun:
                Gun tempGun = new Gun();
                tempGun = JsonUtility.FromJson<Gun>(item.itemData);
                tempGun = GetGunConfig(tempGun.name);
                item.itemData = JsonUtility.ToJson(tempGun);
                return item;
            default:
                return null;
        }    
    }    
}


