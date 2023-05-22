using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnter : MonoBehaviour
{
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
    [SerializeField] GameObject spawn4;
    [SerializeField] GameObject spawn5;
    Vector3 tpPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            switch (gameManager.instance.levelnum)
            {
                case 1:
                    tpPos = spawn1.transform.position;
                    break;
                case 2:
                    tpPos = spawn2.transform.position;
                    break;
                case 3:
                    tpPos = spawn3.transform.position;
                    break;
                case 4:
                    tpPos = spawn4.transform.position;
                    break;
                case 5:
                    tpPos = spawn5.transform.position;
                    break;
            }
            gameManager.instance.playerScript.GetComponent<CharacterController>().enabled = false;
            gameManager.instance.playerScript.transform.position = tpPos;
            gameManager.instance.playerScript.GetComponent<CharacterController>().enabled = true;
        }
    }
}
