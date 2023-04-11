using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] int Timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Timer);
    }
    public void OnTrigger(Collider other)
    {
        IDamage Damageable = other.GetComponent<IDamage>();
        if (Damageable != null)
        {
            Damageable.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

}
