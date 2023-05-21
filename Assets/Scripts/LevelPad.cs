using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPad : MonoBehaviour
{
    [SerializeField] int levelNum;
    public AudioSource sfxLevelPad;

    void Start(){
        sfxLevelPad = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            sfxLevelPad.Play();
            gameManager.instance.ChangeLevel(levelNum);
        }
    }
}
