using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballScript : MonoBehaviour
{
    public float damage = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
