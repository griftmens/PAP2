using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Player player;
    int actualScene;
    public int nextScene;
    // Start is called before the first frame update
    void Start()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;
        nextScene = actualScene + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Me estan tocando");
        if (other.gameObject.tag == "Player" && gameManager.instance.enemiesRemaining <= 0) {
            SceneManager.LoadScene(nextScene);
            gameManager.instance.UpdateGameGoal(0);
            //player.GetComponent<Player>().hp = player.GetComponent<Player>().hpOrig;
            Debug.Log("estoy entrando");
        }
    }
}
