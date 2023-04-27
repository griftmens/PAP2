using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoStats ammoType;
    [SerializeField] int ammoCount = -1;
    [SerializeField] MeshFilter model;
    [SerializeField] MeshRenderer material;

    private void Start()
    {
        model.sharedMesh = ammoType.model.GetComponent<MeshFilter>().sharedMesh;
        material.sharedMaterial = ammoType.model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(ammoCount == -1)
            {
                ammoCount = ammoType.defaultCount;
            }
            gameManager.instance.playerScript.UpdateAmmoCount(ammoCount);
            Destroy(gameObject);
        }
    }

}
