using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int money;
    public int offerings;
    public int levelsCleared;
    public bool medBought;
    public bool heavyBought;

    public PlayerData()
    {
        money = gameManager.instance.playerScript.money;
        offerings = gameManager.instance.playerScript.offerings;
        levelsCleared = gameManager.instance.levelsCleared;
        if (ShopManager.instance)
        {
            medBought = ShopManager.instance.medBought;
            heavyBought = ShopManager.instance.heavyBought;
        }
        else
        {
            medBought= false;
            heavyBought= false;
        }
    }
}
