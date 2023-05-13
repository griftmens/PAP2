using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("----- Prices -----")]
    [SerializeField] int mediumPrice;
    [SerializeField] int heavyPrice;
    public bool medBought, heavyBought;

    [Header("----- Wares -----")]
    public GunStats GunLight;
    public GunStats GunMedium;
    public GunStats GunHeavy;

    [Header("----- UI stuff -----")]
    public ShopUI shopUI;
    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        shopUI.medPrice.text = "$" + mediumPrice.ToString("F0");
        shopUI.heavyPrice.text = "$" + heavyPrice.ToString("F0");
        playerScript = gameManager.instance.playerScript;
    }
    public void BuyMed()
    {
        if (playerScript.money >= mediumPrice && !medBought)
        {
            playerScript.PickupGun(GunMedium);
            playerScript.money -= mediumPrice;
            medBought = true;
        }
        else if (medBought)
        {
            gameManager.instance.StartMessage("Medium weapon already purchased!");
        }
        else
        {
            gameManager.instance.StartMessage("You cannot afford medium weapon");
        }
    }
    public void BuyHeavy()
    {
        if (playerScript.money >= heavyPrice && !heavyBought)
        {
            playerScript.PickupGun(GunHeavy);
            playerScript.money -= heavyPrice;
            heavyBought = true;
        }
        else if (heavyBought)
        {
            gameManager.instance.StartMessage("Heavy weapon already purchased!");
        }
        else
        {
            gameManager.instance.StartMessage("You cannot afford heavy weapon");
        }
    }
}
