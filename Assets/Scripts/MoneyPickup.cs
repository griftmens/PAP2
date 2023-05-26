using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] int moneyCount;
    [SerializeField] AudioClip coinClip;
    [SerializeField] AudioSource coinAud;


    void Start(){
        coinAud.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinAud.PlayOneShot(coinClip, 0.7f);
            gameManager.instance.playerScript.PickupMoney(moneyCount);
            Destroy(gameObject);
        }
    }

}
