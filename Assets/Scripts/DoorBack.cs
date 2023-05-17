using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            transform.position = gameManager.instance.playerSpawnPos.transform.position;
        }
    }
}
