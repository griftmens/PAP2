using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer bossmodel;
    [SerializeField] NavMeshAgent bossagent;
    [SerializeField] Transform bossHeadpos;
    [SerializeField] Transform bossShootpos;

    [Header("-----Stats-----")]
    [SerializeField] int bossHealth;
    [SerializeField] int bossPlayerFaceSpeed;
    [SerializeField] int bossSightAngle;

    [Header("-----Gun Stats-----")]
    [Range((float).1, 5)][SerializeField] float bossFireRate;
    [Range(1, 10)][SerializeField] int bossShotDistance;
    [Range(1, 100)][SerializeField] int bossShotDamage;
    [SerializeField] float bossBulletSpeed;
    [SerializeField] GameObject bossBullet;

    bool bossPlayerinRange;
    bool bossIsShooting;
    Vector3 bossPlayerDirection;
    float bossAngleToPlayer;
    float bossStopDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.UpdateGameGoal(1);
        bossStopDistance = bossagent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossPlayerinRange)
        {
            if (CanSee())
            {

            }
        }
    }
    bool CanSee()
    {
        bossPlayerDirection = (gameManager.instance.player.transform.position - bossHeadpos.position);
        bossAngleToPlayer = Vector3.Angle(new Vector3(bossPlayerDirection.x, 0, bossPlayerDirection.z), transform.forward);
        Debug.DrawRay( bossHeadpos.position, bossPlayerDirection);
        // Debug.Log(AngleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(bossHeadpos.position, bossPlayerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && bossAngleToPlayer <= bossSightAngle)
            {
                bossagent.stoppingDistance = bossStopDistance;
                bossagent.SetDestination(gameManager.instance.player.transform.position);
                if (bossagent.remainingDistance <= bossagent.stoppingDistance)
                {
                    FacePlayer();
                }
                if (!bossIsShooting)
                {
                    StartCoroutine(Shoot());
                }
                return true;
            }
        }
        return false;
    }
    IEnumerator Shoot()
    {
        bossIsShooting = true;
        GameObject bullet = Instantiate(bossBullet, bossShootpos.position, bossBullet.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bossPlayerDirection.normalized * bossBulletSpeed;
        yield return new WaitForSeconds(bossFireRate);
        bossIsShooting = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossPlayerinRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossPlayerinRange = false;
        }
    }
    public void TakeDamage(int damage)
    {
        bossHealth -= damage;
        bossagent.SetDestination(gameManager.instance.player.transform.position);
        bossagent.stoppingDistance = 0;
        StartCoroutine(FlashColor());
        if (bossHealth <= 0)
        {
            if (gameManager.instance.enemiesRemaining > 0)
            {
                gameManager.instance.UpdateGameGoal(-1);
            }
            Destroy(gameObject);
        }
    }
    IEnumerator FlashColor()
    {
        bossmodel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        bossmodel.material.color = Color.white;
    }
    void FacePlayer()
    {
        Quaternion face = Quaternion.LookRotation(new Vector3(bossPlayerDirection.x, 0, bossPlayerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, face, Time.deltaTime * bossPlayerFaceSpeed);
    }
}
