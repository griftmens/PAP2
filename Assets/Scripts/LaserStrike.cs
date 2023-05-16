using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStrike : MonoBehaviour
{
    bool isReady;
    void Start()
    {
        isReady = true;
        Destroy(gameObject, gameManager.instance.playerScript.laserstrikeTime);
    }

    public void OnTriggerStay(Collider other)
    {
        IDamage Damageable = other.GetComponent<IDamage>();
        if (Damageable != null && isReady && other.GetType() == typeof(CapsuleCollider))
        {
            Damageable.TakeDamage(gameManager.instance.playerScript.laserstrikeDamage);
            StartCoroutine(DamageCycle());
        }
    }

    IEnumerator DamageCycle()
    {
        isReady = false;
        yield return new WaitForSeconds(1f);
        isReady = true;
    }
}
