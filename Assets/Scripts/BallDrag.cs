using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDrag : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //rb.drag += 1.0f;
    }
}
