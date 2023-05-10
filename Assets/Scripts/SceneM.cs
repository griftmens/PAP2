using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneM : MonoBehaviour
{
    public GameObject uiMain;
    public GameObject optionMain;
    public GameObject creditsMain;

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
    }

    public void CreditsG(){
        uiMain.SetActive(false);
        creditsMain.SetActive(true);
    }

    public void QuitG() {
        Application.Quit();
    }
}
