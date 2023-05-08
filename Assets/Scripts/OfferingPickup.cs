using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.PickupOffering();
            Destroy(gameObject);
        }
    }
}
