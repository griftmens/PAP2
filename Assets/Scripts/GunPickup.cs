using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;
    [SerializeField] MeshFilter model;
    [SerializeField] MeshRenderer material;

    private void Start()
    {
        model.sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        material.sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.PickupGun(gun);
            Destroy(gameObject);
        }
    }

}
