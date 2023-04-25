using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamage
{
    #region  Headers
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [SerializeField] int hp;
    [SerializeField] float playerSpeed;
    [SerializeField] float sprintMultiplier;

    [Header("----- Player Physics -----")]
    [SerializeField] float gravity;
    [SerializeField] int jumpMaxTimes;
    [SerializeField] int jumpHeight;

    [Header("----- Gun Stats -----")]
    public List<GunStats> gunsInventory = new List<GunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootRange;
    [SerializeField] int shootDamage;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;
    public int selectedGun;

    [Header("----- Stamina -----")]
    [SerializeField] float playerStamina;
    [SerializeField] int maxStamina;
    [Range(0, 50)][SerializeField] int staminaDrain;
    [Range(0, 50)][SerializeField] int staminaRegen;
    [Range(0, 1)][SerializeField] float tiredMultiplier;
    [SerializeField] float tiredTime;
    [SerializeField] bool tired;
    [SerializeField] bool weAreSprinting;
    #endregion

    #region Other Variables
    Vector3 playerVelocity, move;
    bool groundedPlayer, isShooting;
    int jumpedTimes;
    int hpOrig;
    float staminaOrig;
    #endregion

    void Start()
    {
        hpOrig = hp;
        playerStamina = maxStamina;
        staminaOrig = playerStamina;
        //gameManager.instance.HPTotal.text = hp.ToString("F0");
        weAreSprinting = false;
        UIUpdate();
    }

    void Update()
    {
        if (gameManager.instance.activeMenu == null) {
            Movemente();
            SelectGun();
            if (!isShooting && Input.GetButton("Fire1")) {
                StartCoroutine(Shoot());
            }
        }
    }

    void Movemente() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            weAreSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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
    public void TakeDamage(int Damage)
    {
        hp -= Damage;
        UIUpdate();
        if(hp <= 0)
        {
            gameManager.instance.PlayerDead();
        }
    }
    IEnumerator Tired()
    {
        tired = true;
        float speedOG = playerSpeed;
        int jumpMaxOG = jumpMaxTimes;
        float spintMultiOG = sprintMultiplier;
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
        RaycastHit hit;
        if (Physics.Raycast(UnityEngine.Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootRange))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(shootDamage);
            }
        }
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
    void UIUpdate()
    {
        gameManager.instance.HPBar.fillAmount = (float)hp / (float)hpOrig;
        //gameManager.instance.HPCurrent.text = hp.ToString("F0");
        gameManager.instance.StamBar.fillAmount = (float) playerStamina / (float)staminaOrig;
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

        gunModel.sharedMesh = gunStat.model.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.sharedMaterial = gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;
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
        gunModel.mesh = gunsInventory[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.material = gunsInventory[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
