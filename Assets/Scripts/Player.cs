using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamage
{
    #region  Headers
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;


    [Header("----- Player Stats -----")]
    [SerializeField] public int hp;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMultiplier;

    [Header("----- Player Physics -----")]
    [SerializeField] float gravity;
    [SerializeField] int jumpMaxTimes;
    [SerializeField] int jumpHeight;

    [Header("----- Gun Stats -----")]
    public List<GunStats> gunsInventory = new List<GunStats>();
    [SerializeField] float shootRate;
    [SerializeField] float rateLast;
    [SerializeField] float rateTimer;
    [SerializeField] int shootRange;
    [SerializeField] int shootDamage;
    [SerializeField] int shootEnergy;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;
    public int selectedGun;
    [SerializeField] float ammoCount;
    [SerializeField] int ammoMax;
    [SerializeField] float reloadTime;

    [Header("----- Stamina -----")]
    [SerializeField] float playerStamina;
    [SerializeField] int maxStamina;
    [Range(0, 50)][SerializeField] int staminaDrain;
    [Range(0, 50)][SerializeField] int staminaRegen;
    [Range(0, 1)][SerializeField] float tiredMultiplier;
    [SerializeField] float tiredTime;
    [SerializeField] bool tired;
    [SerializeField] bool weAreSprinting;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] [Range(0, 1)] float audStepsVol;

    [SerializeField] AudioClip[] audJump;
    [SerializeField] [Range(0, 1)] float audJumpVol;

    [SerializeField] AudioClip[] audDamage;
    [SerializeField] [Range(0, 1)] float audDamageVol;

    [Header("----- Acquireables -----")]
    public int money;
    public int offerings;
    public int abilities;

    [Header("----- Abilities -----")]
    [SerializeField] int overdriveTime;
    [SerializeField] int overdriveCooldown;
    bool overdriveActive, overdriveWait;
    float overdriveTimer;
    [Header("----- Damage Overlay -----")]
    Image overlay;
    public float durationOverlay;
    public float fadeSpeed;
    public float durationTimer;

    [Header("----- Clips -----")]
    [SerializeField] AudioClip shieldClip;
    [SerializeField] AudioClip absorbClip;
    [SerializeField] AudioClip runningClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioSource shieldAud;
    [SerializeField] AudioSource absorbAud;
    [SerializeField] AudioSource runningAud;
    [SerializeField] AudioSource shootAud;

    [Header("----- Other -----")]
    [SerializeField] int absorptionTime;
    [SerializeField] int absorptionCooldown;
    [SerializeField] int absorbChance;
    [SerializeField] int absorbAmount;
    bool absorptionActive, absorptionWait;
    float absorptionTimer;

    public int laserstrikeTime, laserstrikeDamage;
    [SerializeField] int laserstrikeCooldown;
    bool laserstrikeWait, gettingPosition, laserstrikeActive;
    float laserstrikeTimer;
    [SerializeField] GameObject LaserPredictionPrefab;
    [SerializeField] GameObject LaserStrikePrefab;
    GameObject laserPrediction;

    public int phaseTime;
    [SerializeField] int phaseCooldown;
    bool phaseActive, phaseWait;
    float phaseTimer;

    [SerializeField] int kamikazeChargeTime;
    [SerializeField] int kamikazeCooldown;
    bool kamikazeCharging, kamikazeWait, kamikazeCharged;
    float kamikazeChargeTimer, kamikazeCooldownTimer;
    [SerializeField] GameObject ExplosionPrefab;


    #endregion

    #region Other Variables
    Vector3 playerVelocity, move;
    bool groundedPlayer, isShooting, isPlayingSteps, isReloading;
    int jumpedTimes;
    public int hpOrig;
    float staminaOrig;
    RaycastHit hit;
    bool laserFirst;
    bool isAbilitie;
    float speedOG;
    int jumpMaxOG;
    float spintMultiOG;

    #endregion

    void Start()
    {
        overlay = gameManager.instance.DamageOverlay;
        hpOrig = hp;
        playerStamina = maxStamina;
        staminaOrig = playerStamina;
        ammoCount = ammoMax;
        weAreSprinting = false;
        laserFirst = false;
        RespawnPlayer();
        UIUpdate();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        shieldAud = GetComponent<AudioSource>();
        absorbAud = GetComponent<AudioSource>();
        runningAud = GetComponent<AudioSource>();
        shootAud = GetComponent<AudioSource>();
        speedOG = playerSpeed;
        jumpMaxOG = jumpMaxTimes;
        spintMultiOG = sprintMultiplier;
    }

    void Update()
    {
        if (gameManager.instance.activeMenu == null) {
            Movemente();
            SelectGun();
            if (!isShooting && !isReloading && Input.GetButton("Fire1") && gunsInventory.Count > 0) {
                StartCoroutine(Shoot());
            }
            else if (isShooting)
            {
                FireRateUI();
            }
            if(abilities > 0)
            {
                Abilities();
            }
            if(isReloading)
            {
                Reload();
            }
        }

        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > durationOverlay)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    void Movemente() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            if (!isPlayingSteps && move.normalized.magnitude > 0.5f)
            {
                StartCoroutine(playSteps());
            }
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        if (move != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                weAreSprinting = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                weAreSprinting = false;
            }
        }
        else
        {
            weAreSprinting = false;
        }
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpMaxTimes)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        if (weAreSprinting && !tired)
        {
            if (playerStamina > 0)
            {
                controller.Move(move * Time.deltaTime * playerSpeed * sprintMultiplier);
                Stamina();
            }
            else
                StartCoroutine(Tired());
        }
        else
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            if (playerStamina < maxStamina)
                RegenStamina();
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!weAreSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.3f);

        isPlayingSteps = false;
    }
    public void TakeDamage(int Damage)
    {
        if (!phaseActive)
        {
            hp -= Damage;
            UIUpdate();
            if (hp <= 0)
            {
                gameManager.instance.PlayerDead();
            }
        }
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.2f);
        
    }
    IEnumerator Tired()
    {
        tired = true;
        playerSpeed *= tiredMultiplier;
        jumpMaxTimes = 0;
        yield return new WaitForSeconds(tiredTime);
        playerSpeed = speedOG;
        jumpMaxTimes = jumpMaxOG;
        tired = false;
    }
    IEnumerator Shoot()
    {
        isShooting = true;
        UpdateAmmoCount(-shootEnergy);
        shootAud.PlayOneShot(shootClip, 0.7f);
        if (Physics.Raycast(UnityEngine.Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootRange))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(shootDamage);
            }
        }
        rateLast = shootRate;
        rateTimer = shootRate;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void Stamina()
    {
        playerStamina -= staminaDrain * Time.deltaTime;
        UIUpdate();
    }

    void RegenStamina() {
        playerStamina += staminaRegen * Time.deltaTime;
        UIUpdate();
    }

    void Reload()
    {
        ammoCount += (ammoMax / reloadTime) * Time.deltaTime;
        if(ammoCount >= ammoMax)
        {
            isReloading = false;
        }
        UIUpdate();
    }

    void FireRateUI()
    {
        rateTimer -= Time.deltaTime;
        UIUpdate();
    }

    void UIUpdate()
    {
        gameManager.instance.HPBar.fillAmount = (float)hp / (float)hpOrig;
        gameManager.instance.StamBar.fillAmount = (float)playerStamina / (float)staminaOrig;
        gameManager.instance.ammoBar.fillAmount = (float)ammoCount / (float)ammoMax;
        gameManager.instance.firerateBar.fillAmount = rateTimer / rateLast;
        gameManager.instance.MoneyCount.text = money.ToString("F0");
        gameManager.instance.OfferingCount.text = offerings.ToString("F0");
    }

    public void RespawnPlayer()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        hp = hpOrig;
        UIUpdate();
    }

    public void PickupGun(GunStats gunStat)
    {
        gunsInventory.Add(gunStat);
        shootDamage = gunStat.shootDamage;
        shootRange = gunStat.shootRange;
        shootRate = gunStat.shootRate;
        shootEnergy = gunStat.ammoConsumption;

        gunModel.sharedMesh = gunStat.model.GetComponentInChildren<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunStat.model.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        UIUpdate();
    }

    public void PickupMoney(int moneyAmt)
    {
        money += moneyAmt;
        UIUpdate();
    }

    public void PickupOffering()
    {
        offerings++;
        UIUpdate();
    }

    public void UpdateAmmoCount(int ammo)
    {
        ammoCount += ammo;
        if(ammoCount <= 0.01f)
        {
            isReloading = true;
        }
        UIUpdate();
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunsInventory.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }

    void ChangeGun()
    {
        shootDamage = gunsInventory[selectedGun].shootDamage;
        shootRange = gunsInventory[selectedGun].shootRange;
        shootRate = gunsInventory[selectedGun].shootRate;
        shootEnergy = gunsInventory[selectedGun].ammoConsumption;
        gunModel.mesh = gunsInventory[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.material = gunsInventory[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Abilities()
    {
        // Overdrive

        if(Input.GetKeyDown(KeyCode.E) && !overdriveWait && !phaseActive)
        {
            runningAud.PlayOneShot(runningClip, 0.7f);
            StartCoroutine(Overdrive());
            overdriveTimer = overdriveTime;
        }
        if(overdriveActive)
        {
            overdriveTimer -= Time.deltaTime;
            gameManager.instance.overdrive.fillAmount = overdriveTimer / overdriveTime;
        }
        else if(overdriveWait)
        {
            overdriveTimer += Time.deltaTime;
            gameManager.instance.overdrive.fillAmount = overdriveTimer / overdriveCooldown;
        }

        // Absorption

        if (Input.GetKeyDown(KeyCode.Q) && !absorptionWait && abilities > 1 && !phaseActive)
        {
            absorbAud.PlayOneShot(absorbClip, 0.7f);
            StartCoroutine(Absorption());
            absorptionTimer = absorptionTime;
        }
        if (absorptionActive)
        {
            absorptionTimer -= Time.deltaTime;
            gameManager.instance.absorption.fillAmount = absorptionTimer / absorptionTime;
        }
        else if (absorptionWait)
        {
            absorptionTimer += Time.deltaTime;
            gameManager.instance.absorption.fillAmount = absorptionTimer / absorptionCooldown;
        }

        // Kamikaze

        if(Input.GetKeyDown(KeyCode.F) && !kamikazeWait && abilities > 2 && !phaseActive)
        {
            kamikazeCharging = true;
        }
        if(kamikazeCharging)
        {
            if (kamikazeChargeTimer < kamikazeChargeTime)
            {
                kamikazeChargeTimer += Time.deltaTime;
                gameManager.instance.kamikazeChargeBar.fillAmount = kamikazeChargeTimer / kamikazeChargeTime;
            }
            else
            {
                gameManager.instance.kamikazeChargeBar.color = Color.green;
                kamikazeCharging = false;
                kamikazeCharged = true;
            }
        }
        else if (kamikazeChargeTimer > 0 && !kamikazeCharged && !kamikazeWait)
        {
            kamikazeChargeTimer -= Time.deltaTime;
            gameManager.instance.kamikazeChargeBar.fillAmount = kamikazeChargeTimer / kamikazeChargeTime;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            if(kamikazeCharged)
            {
                kamikazeCharged = false;
                kamikazeChargeTimer = 0;
                gameManager.instance.kamikazeChargeBar.fillAmount = kamikazeChargeTimer / kamikazeChargeTime;
                kamikazeCooldownTimer = 0;
                gameManager.instance.kamikazeChargeBar.color = Color.red;
                StartCoroutine(Kamikaze());
            }
            else if(kamikazeCharging)
            {
                kamikazeCharging = false;
            }
        }
        if(kamikazeWait)
        {
            kamikazeCooldownTimer += Time.deltaTime;
            gameManager.instance.kamikaze.fillAmount = kamikazeCooldownTimer / kamikazeCooldown;
        }

        // Laser Strike
        if (Input.GetKeyDown(KeyCode.V) && abilities > 3 && !laserstrikeWait)
        {
            gettingPosition = !gettingPosition;
            if(!laserFirst)
            {
                laserFirst = true;
                laserPrediction = Instantiate(LaserPredictionPrefab);
            }
            else
                laserPrediction.SetActive(gettingPosition);
        }
        if(gettingPosition)
        {
            if (Physics.Raycast(UnityEngine.Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
            {
                laserPrediction.transform.position = hit.point;
            }
            if(Input.GetButton("Fire1"))
            {
                gettingPosition = false;
                laserPrediction.SetActive(gettingPosition);
                laserstrikeTimer = laserstrikeTime;
                Debug.Log("Audio laser activate");
                StartCoroutine(LaserStrike());
            }
        }
        if (laserstrikeActive)
        {
            laserstrikeTimer -= Time.deltaTime;
            gameManager.instance.laserstrike.fillAmount = laserstrikeTimer / laserstrikeTime;
        }
        else if (laserstrikeWait)
        {
            laserstrikeTimer += Time.deltaTime;
            gameManager.instance.laserstrike.fillAmount = laserstrikeTimer / laserstrikeCooldown;
        }

        // Phase

        if (Input.GetKeyDown(KeyCode.X) && !phaseWait && abilities > 4)
        {
            shieldAud.PlayOneShot(shieldClip, 0.7f);
            StartCoroutine(Phase());
            phaseTimer = phaseTime;
        }
        if (phaseActive)
        {
            phaseTimer -= Time.deltaTime;
            gameManager.instance.phase.fillAmount = phaseTimer / phaseTime;
        }
        else if (phaseWait)
        {
            phaseTimer += Time.deltaTime;
            gameManager.instance.phase.fillAmount = phaseTimer / phaseCooldown;
        }
    }

    IEnumerator Overdrive()
    {
        overdriveWait = true;
        overdriveActive = true;

        playerSpeed *= 2;
        shootRate /= 2;
        yield return new WaitForSeconds(overdriveTime);
        playerSpeed = speedOG;
        shootRate *= 2;

        overdriveActive = false;
        yield return new WaitForSeconds(overdriveCooldown);
        overdriveWait = false;
    }
    IEnumerator Absorption()
    {
        absorptionWait = true;
        absorptionActive = true;

        yield return new WaitForSeconds(absorptionTime);

        absorptionActive = false;
        yield return new WaitForSeconds(absorptionCooldown);
        absorptionWait = false;
    }

    public void Absorb()
    {
        if (absorptionActive)
        {
            int rand = Random.Range(0, absorbChance);
            if (rand == 0)
            {
                TakeDamage(-absorbAmount);
            }
        }
    }

    IEnumerator Kamikaze()
    {
        kamikazeWait = true;
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, new Quaternion());

        yield return new WaitForSeconds(kamikazeCooldown);

        kamikazeWait = false;
    }
    IEnumerator LaserStrike()
    {
        laserstrikeWait = true;
        laserstrikeActive = true;

        GameObject laserStrike = Instantiate(LaserStrikePrefab, laserPrediction.transform.position, new Quaternion());

        yield return new WaitForSeconds(laserstrikeTime);

        laserstrikeActive = false;

        yield return new WaitForSeconds(laserstrikeCooldown);

        laserstrikeWait = false;
    }

    IEnumerator Phase()
    {
        phaseWait = true;
        phaseActive = true;

        gameManager.instance.StartPhase();
        yield return new WaitForSeconds(phaseTime);

        phaseActive = false;
        yield return new WaitForSeconds(phaseCooldown);
        phaseWait = false;
    }
}
