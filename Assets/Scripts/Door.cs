using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Player player;
    int actualScene;
    // Start is called before the first frame update
    void Start()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && gameManager.instance.enemiesRemaining <= 0) {
            SceneManager.LoadScene(actualScene + 1);
            gameManager.instance.playerScript.hp = gameManager.instance.playerScript.hpOrig;
        }
    }
}
