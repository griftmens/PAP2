using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int explosionDamage;
    [SerializeField] int explosionRange;
    [SerializeField, Range(1,3)] float explosionSpeed;

    bool hit;

    void Update()
    {
        Vector3 explosionDistance = new Vector3(explosionRange, explosionRange, explosionRange);
        //transform.localScale = Vector3.Lerp(transform.localScale, explosionDistance, Time.deltaTime * explosionSpeed);

        transform.localScale += explosionSpeed * explosionDistance * Time.deltaTime;

        if (transform.localScale.x >= explosionDistance.x)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        IDamage Damageable = other.GetComponent<IDamage>();
        if (Damageable != null)
        {
            if(other.CompareTag("Player"))
            {
                if (!hit)
                {
                    hit = true;
                    Damageable.TakeDamage(explosionDamage);
                }
            }
            else
                Damageable.TakeDamage(explosionDamage);
        }
    }
}
