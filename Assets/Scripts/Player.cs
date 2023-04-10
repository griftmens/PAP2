using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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
    [SerializeField] float shootRange;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;

    [Header("----- Stamina -----")]
    [SerializeField] float playerStamina;
    [SerializeField] float maxStamina;
    [SerializeField] bool hasRegenerated;
    [SerializeField] bool weAreSprinting;
    [Range(0, 50)][SerializeField] float staminaDrain;
    [Range(0, 50)][SerializeField] float staminaRegen;
    #endregion

    #region Other Variables
    Vector3 playerVelocity, move;
    bool groundedPlayer, isShooting;
    int jumpedTimes;
    int hpOrig;
    #endregion

    void Start()
    {
        hpOrig = hp;
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

        if(Input.GetKey(KeyCode.LeftShift) && playerStamina > 0){
            controller.Move(move * Time.deltaTime * (playerSpeed * 1.2f));
            playerStamina--;
        }
    }
}
