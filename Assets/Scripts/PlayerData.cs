using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int money;
    public int offerings;
    public int levelsCleared;
    public int guns;

    public PlayerData()
    {
        money = gameManager.instance.playerScript.money;
        offerings = gameManager.instance.playerScript.offerings;
        levelsCleared = gameManager.instance.levelsCleared;
        guns = gameManager.instance.playerScript.gunsInventory.Count;
    }
}
