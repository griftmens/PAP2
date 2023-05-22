using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBack : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
        {
            if (gameManager.instance.enemiesRemaining == 0)
            {
                gameManager.instance.Save();
                SceneManager.LoadScene(1);
            }
            else
            {
                gameManager.instance.StartMessage("Robots still engaged, finish the mission.");
            }
        }
    }
}
