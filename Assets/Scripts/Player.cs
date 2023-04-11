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
    [SerializeField] int playerSpeed;

    [Header("----- Player Physics -----")]
    [SerializeField] float gravity;
    [SerializeField] int jumpMaxTimes;
    [SerializeField] int jumpHeight;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootRange;
    [SerializeField] int shootDamage;

    [Header("----- Stamina -----")]
    [SerializeField] int playerStamina;
    [SerializeField] int maxStamina;
    [SerializeField] bool hasRegenerated;
    [SerializeField] bool weAreSprinting;
    [Range(0, 50)][SerializeField] int staminaDrain;
    [Range(0, 50)][SerializeField] int staminaRegen;
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
        //gameManager.instance.HPTotal.text = hp.ToString("F0");
        hasRegenerated = true;
        weAreSprinting = false;
        UIUpdate();
    }

    void Update()
    {
        Movemente();
    }

    void Movemente(){
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0){
            playerVelocity.y = 0f;
            jumpedTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal"))+ (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(Input.GetButtonDown("Jump") && jumpedTimes < jumpMaxTimes){
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // if(Input.GetKey(KeyCode.LeftShift) && playerStamina > 0 && hasRegenerated == true ){
        //     weAreSprinting = true;
        //     controller.Move(move * Time.deltaTime * (playerSpeed * 1.5f));
        //     Stamina();
        // } else if (playerStamina < maxStamina) {
        //     hasRegenerated = false;
        //     weAreSprinting = false;
        //     controller.Move(move * Time.deltaTime * playerSpeed);
        //     RegenStamina();
        // }
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

    // void Stamina() {
    //     playerStamina = (playerStamina - staminaDrain);
    // }

    // void RegenStamina() {
    //     playerStamina += staminaRegen;
    //     if (playerStamina == maxStamina) {
    //         hasRegenerated = true;
    //     }
    // }
    void UIUpdate()
    {
        gameManager.instance.HPBar.fillAmount = (float)hp / (float)hpOrig;
        //gameManager.instance.HPCurrent.text = hp.ToString("F0");
        //gameManager.instance.StamBar.fillAmount = (float)playerStamina / (float)staminaOrig;
    }
}
