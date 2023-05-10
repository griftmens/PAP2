using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneM : MonoBehaviour
{
    public GameObject uiMain;
    public GameObject optionMain;
    public GameObject creditsMain;
    public AudioSource musicMain;
    public AudioSource sfxMain;
    public AudioMixer audioMixer;

     public void StartG(){
        SceneManager.LoadScene(1);
    }

    public void OptionsG(){
        uiMain.SetActive(false);
        optionMain.SetActive(true);
    }

    public void BackG(){
        optionMain.SetActive(false);
        creditsMain.SetActive(false);
        uiMain.SetActive(true);

        if(Input.GetKey(KeyCode.Delete) && (optionMain == true || creditsMain == true)){
            uiMain.SetActive(true);
        }
    }

    public void CreditsG(){
        uiMain.SetActive(false);
        creditsMain.SetActive(true);
    }

    public void QuitG() {
        Application.Quit();
    }

    public void ControlMusic(float sliderMusicM){
        audioMixer.SetFloat("MusicVol", Mathf.Log10(sliderMusicM) * 20);
    }

    public void ControlSFX(float sliderSFXM){
        audioMixer.SetFloat("SFXVol", Mathf.Log10(sliderSFXM) * 20);
    }
}
