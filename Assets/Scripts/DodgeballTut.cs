using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballTut : MonoBehaviour
{
    //Initialize Variables
    public float damage = 10.0f;
    public Rigidbody rb;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        //Destroy(gameObject, 10);
    }
}
