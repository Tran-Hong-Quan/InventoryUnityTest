using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    ItemConfig itemConfig;
    private void Start()
    {
        itemConfig = Inventory.instance.itemConfig;
    }
    public void AddManaToInventory()
    {
        Item mana = new Item(ItemType.ManaFlask, "mana");
        itemConfig.GetItemConfig(ref mana);
        Inventory.instance.AddItemToInventory(mana);
    }
    public void AddBookToInventory()
    {
        Item book = new Item(ItemType.Book, "book");
        book = itemConfig.GetItemConfig(book);
        Inventory.instance.AddItemToInventory(book);
    }
    public void AddGunToInventory()
    {
        Item Gun = new Item(ItemType.Gun, "gun");
        Gun = itemConfig.GetItemConfig(Gun);
        Inventory.instance.AddItemToInventory(Gun);
    }
    public void AddSwordToInventory()
    {
        Item sword = new Item(ItemType.Sword, "sword");
        sword = itemConfig.GetItemConfig(sword);
        Inventory.instance.AddItemToInventory(sword);
    }
    public void AddHPToInventory()
    {
        Item hp = new Item(ItemType.LifeFlask, "hp");
        hp = itemConfig.GetItemConfig(hp);
        Inventory.instance.AddItemToInventory(hp);
    }
    public void AddFoodToInventory()
    {
        Item food = new Item(ItemType.Food, "food");
        food = itemConfig.GetItemConfig(food);
        Inventory.instance.AddItemToInventory(food);
    }
    
}
