using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="ItemConfig",fileName ="Intem Config")]
public class ItemConfig : ScriptableObject
{
    public List<float> manaRcoveryLevelList;
    public float GetManaRecovery(int level)
    {
        return manaRcoveryLevelList[level];
    }

    public List<float> healingLevelList;
    public float GetHealing(int level)
    {
        return healingLevelList[level];
    }

    public List<string> booksData; 
    public string GetBookData(int type)
    {
        return booksData[type];
    }    
    [Serializable]
    public struct GunConfig
    {
        public int damage;
        public int level;
        public int denominations;
        public int maxBullet;
    }    

    public List<GunConfig> gunConfigs;

    public Gun GetGunConfig(int level)
    {
        foreach (GunConfig i in gunConfigs)
        {
            if(i.level == level)
            {
                Gun gun = new Gun();
                gun.damage = i.damage;
                gun.level = level;
                gun.denominations = i.denominations;
                return gun;
            }    
        }

        return null;
    }    
}


