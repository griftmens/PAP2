using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class KamikazeAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform Headpos;
    [SerializeField] Transform Shootpos;
    [SerializeField] AudioSource aud;

    [Header("-----Stats-----")]
    [SerializeField] int Health;
    [SerializeField] int PlayerFaceSpeed;
    [SerializeField] int Damage;
    [SerializeField] int SightAngle;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDistance;
    [SerializeField] float animTransSpeed;
    [SerializeField] GameObject drop;
    [SerializeField] int dropChance;
    [SerializeField] int selfdestructtime;
    [SerializeField] GameObject Explosionprefab;

    bool PlayerinRange;
    bool isexploding;
    Vector3 PlayerDirection;
    float AngleToPlayer;
    float StopDistance;
    bool destinationChosen;
    Vector3 startingPos;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.UpdateGameGoal(1);
        StopDistance = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * 3);
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            if (PlayerinRange && !CanSee())
            {
                StartCoroutine(Roam());
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
                StartCoroutine(Roam());
        }
    }
    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);
            destinationChosen = false;

            Vector3 randPos = Random.insideUnitSphere * roamDistance;
            randPos += startingPos;

            NavMesh.SamplePosition(randPos, out NavMeshHit hit, roamDistance, 1);

            agent.SetDestination(hit.position);
            destinationChosen = false;
        }
    }
    bool CanSee()
    {
        PlayerDirection = (gameManager.instance.player.transform.position - Headpos.position);
        AngleToPlayer = Vector3.Angle(new Vector3(PlayerDirection.x, 0, PlayerDirection.z), transform.forward);
        Debug.DrawRay(Headpos.position, PlayerDirection);
        RaycastHit hit;
        if (Physics.Raycast(Headpos.position, PlayerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && AngleToPlayer <= SightAngle)
            {
                agent.stoppingDistance = StopDistance;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
                return true;
            }
        }
        return false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinRange = true;
            Selfdestruct();
           
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinRange = false;
            agent.stoppingDistance = 0;
        }
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            StopAllCoroutines();
            if (drop)
            {
                int rand = Random.Range(0, dropChance);
                if (rand == 0)
                    Instantiate(drop, transform.position, drop.transform.rotation);
            }
            gameManager.instance.UpdateGameGoal(-1);
            anim.SetBool("Dead", true);
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
        }
        else
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            agent.stoppingDistance = 0;
            anim.SetTrigger("Damage");
            StartCoroutine(FlashColor());
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
    IEnumerator Selfdestruct()
    {
        isexploding = true;
        yield return new WaitForSeconds(selfdestructtime);
        GameObject boom = Instantiate(Explosionprefab, transform.position, new Quaternion());
        StopAllCoroutines();
        gameManager.instance.UpdateGameGoal(-1);
        anim.SetBool("Dead", true);
        GetComponent<CapsuleCollider>().enabled = false;
        agent.enabled = false;

        
    }
}
