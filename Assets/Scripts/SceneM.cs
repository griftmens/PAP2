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
    public GameObject showCase;
    public AudioSource musicMain;
    public AudioSource sfxMain;
    public AudioMixer audioMixer;
    public AudioMixer sfxMixer;
    public AudioClip clickSound;

    public Slider volumeSFX;
    public Slider volumeMusic;
    public void DeleteSave()
    {
        gameManager.instance.DeleteSave();
    }

     public void StartG(){
        Time.timeScale = 1;
        PlaySound();
        SceneManager.LoadScene(1);
    }

    public void OptionsG(){
        uiMain.SetActive(false);
        optionMain.SetActive(true);
        PlaySound();
    }

    public void BackG(){
        optionMain.SetActive(false);
        creditsMain.SetActive(false);
        uiMain.SetActive(true);
        PlaySound();
    }

    public void CreditsG(){
        PlaySound();
        uiMain.SetActive(false);
        creditsMain.SetActive(true);
    }

    public void QuitG() {
        PlaySound();
        Application.Quit();
    }

    public void ControlMusic(float sliderMusicM){
        audioMixer.SetFloat("GameMusicVol", Mathf.Log10(sliderMusicM) * 20);
    }

    public void ControlSFX(float sliderSFXM){
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(sliderSFXM) * 20);
    }

    public void OpenShowCase(){
        SceneManager.LoadScene(4);
    }

    public void PlaySound(){
        sfxMain.PlayOneShot(clickSound);
    }
}
