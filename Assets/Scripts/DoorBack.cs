using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBack : MonoBehaviour
{
    Vector3 backLob;
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            backLob = gameManager.instance.playerSpawnPos.transform.position;
        }

        gameManager.instance.playerScript.GetComponent<CharacterController>().enabled = false;
        gameManager.instance.playerScript.transform.position = backLob;
        gameManager.instance.playerScript.GetComponent<CharacterController>().enabled = true;
    }
}
