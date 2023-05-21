using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LaserStrike : MonoBehaviour
{
    bool isReady;
    public AudioSource laserSound;
    public AudioClip laserClip;
    void Start()
    {
        isReady = true;
        Destroy(gameObject, gameManager.instance.playerScript.laserstrikeTime);
        laserSound = GetComponent<AudioSource>();
    }

    public void OnTriggerStay(Collider other)
    {
        laserSound.PlayOneShot(laserClip, 0.4f);
        //Debug.Log("Sound activate");
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
