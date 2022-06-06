using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public ItemType itemType;
    public string itemData;
}

[Serializable]
public enum ItemType
{
    sword=0,
    gun=1,
    coin=2,
    lifeFlask=3,
    manaFlask=4,
    bullet=5,
    book=6,
    food=7,
}

[Serializable]
public class ConsumableItem
{
    public string name;
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
public class Food : ConsumableItem
{
    public float hungerFill;
}

[Serializable]
public class Coin
{
    public string name;
    public int amount;
}

[Serializable]
public class Weapon
{
    public string name;
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
    public int amount;
    public string name;
    public string data;
}


