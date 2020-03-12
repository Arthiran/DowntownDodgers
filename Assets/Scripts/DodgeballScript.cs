using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballScript : MonoBehaviour
{
    //Initialize Variables
    public float GravitationalForce = 25.0f;
    public float damage = 10.0f;
    public Rigidbody rb;
    public int PlayerID = 0;
    public bool inAir;
    private GameObject BoxTrigger;
    private float speedFactor = 100.0f;
    [HideInInspector]
    public GameObject parentGameObject;

    public PlayerMovementController MovementController;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
        inAir = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.name == "BoxTrigger" && parentGameObject != collision.GetComponentInParent<PlayerRootInfo>().gameObject)
        {
            BoxTrigger = collision.gameObject;
            rb.AddForce((BoxTrigger.transform.position - transform.position) * speedFactor);
        }
        //If the object collieded with has a Target script
        Target target = collision.gameObject.GetComponentInParent<Target>();
        //If it does, apply the damage to the object's health
        if (target != null)
        {
            if (inAir)
            {
                if (collision.gameObject.GetComponentInParent<PlayerMovementController>().PlayerID != PlayerID)
                {
                    float step = GravitationalForce * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, collision.gameObject.transform.position, step);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        inAir = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        inAir = false;
    }     
}
