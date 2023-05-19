using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMusic : MonoBehaviour
{

    public AudioSource musicGame;
    public AudioMixer musicMixer;
    public AudioSource sfxGame;
    public AudioMixer sfxMixer;

    public void ControlMusicGame(float sliderMusicG){
        musicMixer.SetFloat("GameMusicVol", Mathf.Log10(sliderMusicG) * 10);
    }

    public void ControlSFXGame(float sliderSFXGame){
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(sliderSFXGame) * 10);
    }
}
