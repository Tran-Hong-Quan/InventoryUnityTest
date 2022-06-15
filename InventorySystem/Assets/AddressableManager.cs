using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddressableManager : MonoBehaviour 
{
    public AddressableManager instance;
    public Action<Sprite> OnLoadingItemSuccessfully;
    public Action OnLoadingItemFailed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }



}
