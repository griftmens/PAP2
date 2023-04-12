using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" && gameManager.instance.enemiesRemaining <= 0) {
            SceneManager.LoadScene("Joaquin Test");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && gameManager.instance.enemiesRemaining <= 0) {
            gameManager.instance.UpdateGameGoal(0);
        }
    }
}
