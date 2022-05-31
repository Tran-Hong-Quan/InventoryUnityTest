using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public string itemName;
    public ItemType itemType;
    public string itemData;
}

public enum ItemType
{
    sword=0,
    gun=1,
    coin=2,
    lifeFlask=3,
    manaFlask=4,
    bullet=5,
    book=6,
}

[Serializable]
public class ConsumableItem
{
    public int amount;
    public int maxAmount;
    public int level;
    public int denominations;
}

[Serializable]
public class ManaFlask:ConsumableItem
{
    public float manaRecovery;
}

[Serializable]
public class HealFlask : ConsumableItem
{
    public float manaRecovery;
}

[Serializable]
public class Coin
{
    public int amount;
}

[Serializable]
public class Weapon
{
    public int damage;
    public int level;
    public int denominations;
}

[Serializable]
public class Sword:Weapon
{
   
}

[Serializable]
public class Gun : Weapon
{
    public int bullet;
    public int maxBullet;
}

[Serializable]
public class Book
{
    public string data;
    public int type;
}

