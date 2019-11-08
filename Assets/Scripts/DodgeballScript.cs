using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballScript : MonoBehaviour
{
    //Initialize Variables
    public float damage = 10.0f;
    public Rigidbody rb;

    public bool inAir;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        Destroy(gameObject, 10);

        inAir = true;
    }

    /*private void OnTriggerEnter(Collider collision)
    {
        //If the object collieded with has a Target script
        Target target = collision.gameObject.GetComponent<Target>();
        //If it does, apply the damage to the object's health
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        //Destroys the dodgeball on impact
        Destroy(gameObject);
    }*/
    private void OnCollisionExit(Collision collision)
    {
        inAir = true;
        Debug.Log(inAir);
    }
    private void OnCollisionStay(Collision collision)
    {
        //rb.AddForce(-gameObject.transform.forward * 20, ForceMode.VelocityChange);
        if (collision.gameObject.tag == "Ground")
        {
            inAir = false;
            Debug.Log(inAir);
        }
    }     
}
