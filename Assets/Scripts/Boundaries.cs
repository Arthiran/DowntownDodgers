using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private bool OB; //OB means Out of Bounds
    public float killTimer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OB)
        {
            killTimer -= Time.deltaTime;
            //Debug.Log(killTimer);
            if (killTimer <= 0.0f)
            {
                Debug.Log("DIE");
                this.gameObject.GetComponent<Target>().Die();

                killTimer = 5.0f;
                OB = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            killTimer = 5.0f;
            OB = true;
            Debug.Log("DQ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            OB = false;
            Debug.Log("NO DQ");
        }
    }


}
