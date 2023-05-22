using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPad : MonoBehaviour
{
    [SerializeField] int levelNum;
    public AudioSource sfxLevelPad;
    public string selectedStage;

    void Start(){
        sfxLevelPad = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            sfxLevelPad.Play();

            switch(levelNum){
                case 1:
                    gameManager.instance.StartMessage("Stage 1 Selected");
                    break;
                case 2:
                    gameManager.instance.StartMessage("Stage 2 Selected");
                    break;
                case 3:
                    gameManager.instance.StartMessage("Stage 3 Selected");
                    break;
                case 4:
                    gameManager.instance.StartMessage("Stage 4 Selected");
                    break;
                case 5:
                    gameManager.instance.StartMessage("Stage 5 Selected");
                    break;
            }

            gameManager.instance.levelnum = levelNum;
        }
    }
}
