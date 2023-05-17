using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMusic : MonoBehaviour
{

    public AudioSource musicGame;
    public AudioMixer musicMixer;

    public void ControlMusicGame(float sliderMusicG){
        musicMixer.SetFloat("GameMusicVol", Mathf.Log10(sliderMusicG) * 10);
    }
}
