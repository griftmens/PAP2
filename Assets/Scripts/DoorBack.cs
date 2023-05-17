using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBack : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            gameManager.instance.playerScript.RespawnPlayer();
        }
    }
}
