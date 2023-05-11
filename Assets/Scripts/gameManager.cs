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

    [Header("----- UI Elements / Menus -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject optionMenu;
    public GameObject shopMenu;
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

    [Header("----- Weapons -----")]
    [SerializeField] GameObject GunLight;
    [SerializeField] GameObject GunMedium;
    [SerializeField] GameObject GunHeavy;

    public bool isPaused;
    float timeScaleOg;
    Door dScene;

    void Awake() // 
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOg = Time.timeScale;
        optionMenu.SetActive(false);
        Load();
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

    public void SpawnGunMed()
    {
        Instantiate(GunMedium, playerScript.transform);
        playerScript.money -= 10;
    }
    public void SpawnGunHeavy()
    {
        Instantiate(GunHeavy, playerScript.transform);
        playerScript.money -= 20;
    }

    public void Shop()
    {
        activeMenu = shopMenu;
        activeMenu.SetActive(true);
        Pause();
    }

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

    public void Load()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            playerScript.money = data.money;
            levelsCleared = data.levelsCleared;
            playerScript.offerings = data.offerings;
            Instantiate(GunLight, playerSpawnPos.transform);
            if (data.guns > 1)
            {
                Instantiate(GunMedium, playerSpawnPos.transform);
                if (data.guns > 2)
                {
                    Instantiate(GunHeavy, playerSpawnPos.transform);
                }
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

        if(enemiesRemaining <= 0)
        {
            SaveSystem.SavePlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
}
