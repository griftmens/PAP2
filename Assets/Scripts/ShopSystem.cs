using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    bool shopping;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !shopping)
        {
            StartCoroutine(Shop());
        }
    }

    IEnumerator Shop()
    {
        shopping = true;
        gameManager.instance.Shop();
        yield return new WaitForSeconds(1);
        shopping = false;
    }

    public void BuyMed()
    {
        if(gameManager.instance.playerScript.money >= 10)
        {
            gameManager.instance.SpawnGunMed();
        }
    }
    public void BuyHeavy()
    {
        if (gameManager.instance.playerScript.money >= 20)
        {
            gameManager.instance.SpawnGunHeavy();
        }
    }

}
