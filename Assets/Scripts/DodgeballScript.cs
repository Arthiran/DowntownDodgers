using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballScript : MonoBehaviour
{
    //Initialize Variables
    public float damage = 10.0f;

    //Checks if dodgeball has entered a collider
    private void OnTriggerEnter(Collider other)
    {
        //If the object collieded with has a Target script
        Target target = other.gameObject.GetComponent<Target>();
        //If it does, apply the damage to the object's health
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        //Destroys the dodgeball on impact
        Destroy(gameObject);
    }
}
