using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPad : MonoBehaviour
{
    bool shopping;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !shopping)
        {
            shopping = true;
            gameManager.instance.Shop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && shopping)
            shopping = false;
    }
}
