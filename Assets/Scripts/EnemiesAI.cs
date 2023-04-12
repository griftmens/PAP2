using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform Headpos;
    [SerializeField] Transform Shootpos;

    [Header("-----Stats-----")]
    [SerializeField] int Health;
    [SerializeField] int PlayerFaceSpeed;
    [SerializeField] int SightAngle;

    [Header("-----Gun Stats-----")]
    [Range((float).1, 1)][SerializeField] float FireRate;
    [Range(1,10)][SerializeField] int ShotDistance;
    [Range(1, 100)][SerializeField] int ShotDamage;
    [SerializeField] float BulletSpeed;
    [SerializeField] GameObject Bullet;

    bool PlayerinRange;
    bool IsShooting;
    Vector3 PlayerDirection;
    float AngleToPlayer;
    float StopDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.UpdateGameGoal(1);
        StopDistance = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerinRange)
        {
            if (CanSee())
            {

            }
        }
    }
    bool CanSee()
    {
        PlayerDirection = (gameManager.instance.player.transform.position - Headpos.position);
        AngleToPlayer = Vector3.Angle(new Vector3(PlayerDirection.x, 0, PlayerDirection.z), transform.forward);
        Debug.DrawRay(Headpos.position, PlayerDirection);
        // Debug.Log(AngleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(Headpos.position, PlayerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && AngleToPlayer <= SightAngle)
            {
                agent.stoppingDistance = StopDistance;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
                if (!IsShooting)
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
        IsShooting= true;
        GameObject bullet = Instantiate(Bullet, Shootpos.position, Bullet.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = PlayerDirection.normalized * BulletSpeed;
        yield return new WaitForSeconds(FireRate);
        IsShooting= false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinRange= false;
        }
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        agent.SetDestination(gameManager.instance.player.transform.position);
        agent.stoppingDistance = 0;
        StartCoroutine(FlashColor());
        if(Health <= 0)
        {
            if (gameManager.instance.enemiesRemaining > 0 ) {
                gameManager.instance.UpdateGameGoal(-1);
            }
            Destroy(gameObject);
        }
    }
    IEnumerator FlashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    void FacePlayer()
    {
        Quaternion face = Quaternion.LookRotation(new Vector3(PlayerDirection.x, 0, PlayerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, face, Time.deltaTime * PlayerFaceSpeed);
    }
}
