using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;


    [Header("----- Player -----")]
    public GameObject player;
    public Player playerScript;

    [Header("----- UI Elements / Menus -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public Image HPBar;
    public TextMeshProUGUI HPCurrent;
    public TextMeshProUGUI HPTotal;
    public Image StamBar;
    public TextMeshProUGUI enemiesRemainingText;

    public int enemiesRemaining;

    public bool isPaused;
    float timeScaleOg;

    void Awake() // 
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        timeScaleOg = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Resume()
    {
        Time.timeScale = timeScaleOg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");

        if (enemiesRemaining <= 0)
        {
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            Pause();
        }
    }

    public void PlayerDead()
    {
        Pause();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
}
