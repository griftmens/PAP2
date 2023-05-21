using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Explosion : MonoBehaviour
{
    [SerializeField] int explosionDamage;
    [SerializeField] int explosionRange;
    [SerializeField, Range(1, 3)] float explosionSpeed;
    public AudioSource explosionSFX;
    public AudioClip explosionClip;

    bool hit;

    void Start(){
        explosionSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 explosionDistance = new Vector3(explosionRange, explosionRange, explosionRange);
        explosionSFX.PlayOneShot(explosionClip, 0.4f);
        transform.localScale += explosionSpeed * explosionDistance * Time.deltaTime;

        if (transform.localScale.x >= explosionDistance.x)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                if (!hit)
                {
                    hit = true;
                    damageable.TakeDamage(explosionDamage);
                }
            }
            else if(other.GetType() == typeof(CapsuleCollider))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
    }
}
