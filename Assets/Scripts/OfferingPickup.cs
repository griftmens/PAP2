using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.playerScript.PickupOffering();
        Destroy(gameObject);
    }
}
