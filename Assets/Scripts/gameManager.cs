using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public Player playerScript;
    public GameObject playerSpawnPos;
    public int levelsCleared;
    public int playerOfferings;

    [Header("----- Shop System -----")]
    public GameObject shopMenu;

    [Header("----- UI Elements / Menus -----")]
    public GameObject activeMenu;
    public TextMeshProUGUI message;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject optionMenu;
    public Image HPBar;
    public TextMeshProUGUI HPCurrent;
    public TextMeshProUGUI HPTotal;
    public Image StamBar;
    public Image ammoBar;
    public Image firerateBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI MoneyCount;
    public TextMeshProUGUI OfferingCount;
    public Image overdrive;
    public Image absorption;
    public Image kamikaze;
    public Image kamikazeChargeBar;
    public Image laserstrike;
    public Image phase;
    public GameObject phaseUI;
    public AudioSource musicGame;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public AudioSource sfxGame;
    public AudioClip clickSoungG;

    public int enemiesRemaining;

    [Header("----- Level stuff -----")]
    public int levelnum;
    [SerializeField] GameObject Level1;
    [SerializeField] GameObject Level2;
    [SerializeField] GameObject Level3;
    [SerializeField] GameObject Level4;
    [SerializeField] GameObject Level5;

    public bool isPaused;
    float timeScaleOg;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOg = Time.timeScale;
        optionMenu.SetActive(false);
        StartCoroutine(StartLoad());
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

    public void Shop()
    {
        activeMenu = shopMenu;
        activeMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartMessage(string text)
    {
        StartCoroutine(Message(text));
    }

    IEnumerator Message(string text)
    {
        message.text = text;
        yield return new WaitForSeconds(1);
        message.text = "";
    }

    // Functions

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Resume()
    {
        PlaySound();
        Time.timeScale = timeScaleOg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //GetComponent<AudioSource>().Play();
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void Restart() {
        PlaySound();
        Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void QuitGame(){
        // foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
        //      Destroy(o);
        // }
        //GetComponent<AudioSource>().Play();
        PlaySound();
        SceneManager.LoadScene(0);
    }

    // Save System

    public void Save()
    {
        //GetComponent<AudioSource>().Play();
        PlaySound();
        SaveSystem.SavePlayer();
    }

    public void DeleteSave()
    {
        levelsCleared = 0;
        playerScript.offerings = 0;
        playerScript.money = 0;
        playerScript.gunsInventory.Clear();
        SaveSystem.SavePlayer();
    }

    IEnumerator StartLoad()
    {
        yield return new WaitForSeconds(0.1f);
        Load();
    }

    public void Load()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            playerScript.money = data.money;
            levelsCleared = data.levelsCleared;
            playerScript.offerings = data.offerings;
            playerScript.PickupGun(ShopManager.instance.GunLight);
            if (data.medBought)
            {
                playerScript.PickupGun(ShopManager.instance.GunMedium);
            }

            if (data.heavyBought)
            {
                playerScript.PickupGun(ShopManager.instance.GunHeavy);
            }
        }
    }

    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");

        if(amount == -1)
        {
            playerScript.Absorb();
        }

        //temp win code

        //if(enemiesRemaining <= 0)
        //{
        //    SaveSystem.SavePlayer();
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }

    public void PlayerDead()
    {
        Pause();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public void StartWin()
    {
        StartCoroutine(PlayerWin());
    }

    IEnumerator PlayerWin()
    {
        yield return new WaitForSeconds(3);
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        Pause();
    }

    public void StartPhase()
    {
        StartCoroutine(PlayerPhase());
    }

    IEnumerator PlayerPhase()
    {
        phaseUI.SetActive(true);
        yield return new WaitForSeconds(playerScript.phaseTime);
        phaseUI.SetActive(false);
    }

    public void Option(){
        PlaySound();
        activeMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void Back(){
        PlaySound();
        optionMenu.SetActive(false);
        activeMenu.SetActive(true);
    }

    public void ControlMusicGame(float sliderMusicG){
        musicMixer.SetFloat("GameMusicVol", Mathf.Log10(sliderMusicG) * 20);
    }

    public void ControlSFXGame(float sliderSFX){
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(sliderSFX) * 20);
    }

    public void PlaySound(){
        sfxGame.PlayOneShot(clickSoungG);
    }

    public void ChangeLevel(int level)
    {
        levelnum = level;
        switch (level)
        {
            case 1:
                Level1.SetActive(true); 
                Level2.SetActive(false);
                Level3.SetActive(false);
                Level4.SetActive(false);
                Level5.SetActive(false);
                break;
            case 2:
                Level1.SetActive(false);
                Level2.SetActive(true);
                Level3.SetActive(false);
                Level4.SetActive(false);
                Level5.SetActive(false);
                break;
            case 3:
                Level1.SetActive(false);
                Level2.SetActive(false);
                Level3.SetActive(true);
                Level4.SetActive(false);
                Level5.SetActive(false);
                break;
            case 4:
                Level1.SetActive(false);
                Level2.SetActive(false);
                Level3.SetActive(false);
                Level4.SetActive(true);
                Level5.SetActive(false);
                break;
            case 5:
                Level1.SetActive(false);
                Level2.SetActive(false);
                Level3.SetActive(false);
                Level4.SetActive(false);
                Level5.SetActive(true);
                break;
        }
    }

}
